#nullable enable
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using Microsoft.SemanticKernel.Text;
using System;
using System.Threading.Tasks;
using static UnityEditor.Progress;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    public sealed class AzureCognitiveServicesSpeechSkill
    {
        private readonly SpeechRecognizer speechRecognizer;
        private readonly SpeechSynthesizer speechSynthesizer;
        private readonly string voiceName = "en-US-JennyNeural";
        private readonly string speechRate = "1.0";

        public AzureCognitiveServicesSpeechSkill(
            string subscriptionKey,
            string serviceRegion,
            string? voiceName,
            string? speechRate
        ) {
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, serviceRegion);
            speechConfig.SetProperty(PropertyId.SpeechServiceResponse_PostProcessingOption, "TrueText");

            if (!string.IsNullOrWhiteSpace(voiceName))
            {
                speechConfig.SpeechSynthesisVoiceName = voiceName;
                this.voiceName = voiceName;
            }

            if (!string.IsNullOrWhiteSpace(speechRate)) {
                this.speechRate = speechRate;
            }

            speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
            speechSynthesizer = new SpeechSynthesizer(speechConfig);
        }

        public async Task<string> ListenAsync(SKContext context)
        {
            while(!context.CancellationToken.IsCancellationRequested)
            {
                var result = await speechRecognizer.RecognizeOnceAsync();

                switch (result.Reason)
                {
                    case ResultReason.RecognizedSpeech:
                        return result.Text;
                    case ResultReason.Canceled:
                        var cancelDetails = CancellationDetails.FromResult(result);
                        throw new Exception($"{cancelDetails.Reason}: {cancelDetails.ErrorCode}");
                }

            }
            return string.Empty;
        }

        public async Task SpeakAsync(string message, SKContext context)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var ssml = GenerateSsml(
                    message,
                    "neutral",
                    voiceName,
                    speechRate
                );
                await speechSynthesizer.SpeakSsmlAsync(ssml);
            }
        }

        private string GenerateSsml(string message, string style, string voiceName, string speechRate) => 
            "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"https://www.w3.org/2001/mstts\" xml:lang=\"en-US\">" +
                $"<voice name=\"{voiceName}\">" +
                    $"<prosody rate=\"{speechRate}\">" +
                        $"<mstts:express-as style=\"{style}\">" +
                            $"{message}" +
                        "</mstts:express-as>" +
                    "</prosody>" +
                "</voice>" +
            "</speak>";
    }
}
