#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using UnityEngine;

namespace Microsoft.Prototyping.AzureSpeech
{
    internal class AzureTextToSpeechAPI
    {
        private readonly static string defaultSpeechSynthesisVoiceName = "en-US-JennyNeural";

        public static IEnumerator CreateAsync(
            string input,


            System.Action<AzureTextToSpeechResponse> callback,
            AzureSpeechParameters? parameters
        )
        {
            var subscriptionKey = parameters?.SubscriptionKey;
            var serviceRegion = parameters?.ServiceRegion;

            var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, serviceRegion);

            speechConfig.SpeechSynthesisVoiceName = parameters?.SpeechSynthesisVoiceName ?? defaultSpeechSynthesisVoiceName;

            var speechSynthesizer = new SpeechSynthesizer(speechConfig, null);

            var visemeList = new List<Viseme>();
            speechSynthesizer.VisemeReceived += (s, e) =>
            {
                visemeList.Add(new Viseme(e.VisemeId, e.AudioOffset));
            };

            var task = Task.Run(async () =>
            {
                SpeechSynthesisResult result;
                if (input.StartsWith("<"))
                {
                    result = await speechSynthesizer.SpeakSsmlAsync(input);
                }
                else
                {
                    input =
                        "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"https://www.w3.org/2001/mstts\" xml:lang=\"en-US\">" +
                            $"<voice name=\"{speechConfig.SpeechSynthesisVoiceName}\">" +
                                $"<mstts:express-as style=\"{parameters?.SpeechSynthesisStyle ?? "default"}\">" +
                                    $"<prosody rate=\"{parameters?.SpeechSynthesisRate ?? "1.0"}\">{input}</prosody>" +
                                $"</mstts:express-as>" +
                            $"</voice>" +
                        "</speak>";
                    result = await speechSynthesizer.SpeakSsmlAsync(input);
                }
                speechSynthesizer.Dispose();
                callback?.Invoke(new AzureTextToSpeechResponse() { VisemeList = visemeList, Result = result });
            });

            yield return new WaitUntil(() => task.IsCompleted);
        }
    }
}
