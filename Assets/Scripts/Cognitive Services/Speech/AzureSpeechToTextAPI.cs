#nullable enable
using System.Collections;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using UnityEngine;

namespace Scripts.CognitiveServices.Speech
{
    internal class AzureSpeechToTextAPI
    {
        private readonly static string defaultSpeechRecognitionLanguage = "en-US";

        public static IEnumerator CreateAsync(
            System.Action<SpeechRecognitionResult>? callback,
            AzureSpeechParameters? parameters
        )
        {
            var subscriptionKey = parameters?.SubscriptionKey;
            var serviceRegion = parameters?.ServiceRegion;

            var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, serviceRegion);
            
            speechConfig.SpeechRecognitionLanguage = parameters?.SpeechRecognitionLanguage ?? defaultSpeechRecognitionLanguage;

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            var task = Task.Run(async () =>
            {
                var result = await speechRecognizer.RecognizeOnceAsync();
                callback?.Invoke(result);
            });

            yield return new WaitUntil(() => task.IsCompleted);
        }
    }
}
