#nullable enable
using System.Collections;
using Microsoft.CognitiveServices.Speech;
using Unity.VisualScripting;

namespace Scripts.CognitiveServices.Speech
{
    [UnitCategory("Azure Speech")]
    [UnitTitle("Azure Speech To Text")]
    [UnitSurtitle("Azure Speech")]
    public sealed class AzureSpeechToText : WaitUnit
    {
        [DoNotSerialize]
        public ValueInput? subscriptionKey;

        [DoNotSerialize]
        public ValueInput? serviceRegion;

        [DoNotSerialize]
        public ValueOutput? output;

        private SpeechRecognitionResult? speechRecognitionResult;

        protected override void Definition()
        {
            base.Definition();

            subscriptionKey = ValueInput<string>("subscriptionKey");
            serviceRegion = ValueInput<string>("serviceRegion");

            output = ValueOutput<string>("output", (flow) =>
            {
                if (speechRecognitionResult == null) return "";
                var text = speechRecognitionResult.Text;
                return text;
            });

            Requirement(subscriptionKey, enter);
            Requirement(serviceRegion, enter);
        }

        protected override IEnumerator Await(Flow flow)
        {
            var subscriptionKeyValue = flow.GetValue<string>(subscriptionKey);
            var serviceRegionValue = flow.GetValue<string>(serviceRegion);

            yield return AzureSpeechToTextAPI.CreateAsync(
                (result) => speechRecognitionResult = result,
                new AzureSpeechParameters()
                {
                    SubscriptionKey = subscriptionKeyValue,
                    ServiceRegion = serviceRegionValue,
                }
            );

            yield return exit;
        }
    }
}
