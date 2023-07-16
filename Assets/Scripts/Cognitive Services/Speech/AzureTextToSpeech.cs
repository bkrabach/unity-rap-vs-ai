#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace Microsoft.Prototyping.AzureSpeech
{
    [UnitCategory("Azure Speech")]
    [UnitTitle("Azure Text To Speech")]
    public sealed class AzureTextToSpeech : WaitUnit
    {
        [DoNotSerialize]
        public ValueInput? subscriptionKey;

        [DoNotSerialize]
        public ValueInput? serviceRegion;

        [DoNotSerialize]
        public ValueInput? input;

        [DoNotSerialize]
        public ValueInput? voice;

        [DoNotSerialize]
        public ValueInput? voiceOverride;

        [DoNotSerialize]
        public ValueInput? rate;

        [DoNotSerialize]
        public ValueInput? style;

        [DoNotSerialize]
        public ValueOutput? output;

        [DoNotSerialize]
        public ValueOutput? visemeList;

        private AzureTextToSpeechResponse? speechSynthesisResult;

        protected override void Definition()
        {
            base.Definition();

            subscriptionKey = ValueInput<string>("subscriptionKey");
            serviceRegion = ValueInput<string>("serviceRegion");
            input = ValueInput<string>("input");
            voice = ValueInput<VoiceStyle_en_US_Neural>("voice", VoiceStyle_en_US_Neural.Jenny);
            voiceOverride = ValueInput<string>("voiceOverride", "");
            rate = ValueInput<string>("rate", "");
            style = ValueInput<string>("style", "");

            output = ValueOutput<AudioClip?>("output", (flow) =>
            {
                if (speechSynthesisResult == null) return null;
                var audioClip = WavUtility.ToAudioClip(speechSynthesisResult.Result.AudioData);
                return audioClip;
            });

            visemeList = ValueOutput<List<Viseme>?>("visimeList", (flow) =>
            {
                if (speechSynthesisResult == null) return null;
                return speechSynthesisResult.VisemeList;
            });

            Requirement(subscriptionKey, enter);
            Requirement(serviceRegion, enter);
            Requirement(input, enter);
        }

        protected override IEnumerator Await(Flow flow)
        {
            var subscriptionKeyValue = flow.GetValue<string>(subscriptionKey);
            var serviceRegionValue = flow.GetValue<string>(serviceRegion);
            var inputValue = flow.GetValue<string>(input);
            var voiceValue = VoiceStyle.GetValue(flow.GetValue<VoiceStyle_en_US_Neural>(voice));
            var voiceOverrideValue = flow.GetValue<string>(voiceOverride);
            var rateValue = flow.GetValue<string>(rate);
            var styleValue = flow.GetValue<string>(style);

            var voiceName = string.IsNullOrWhiteSpace(voiceOverrideValue) ? voiceValue : voiceOverrideValue;

            yield return AzureTextToSpeechAPI.CreateAsync(
                inputValue,
                (result) => speechSynthesisResult = result,
                new AzureSpeechParameters()
                {
                    SpeechSynthesisVoiceName = voiceName,
                    SubscriptionKey = subscriptionKeyValue,
                    ServiceRegion = serviceRegionValue,
                    SpeechSynthesisRate = rateValue,
                    SpeechSynthesisStyle = styleValue
                }
            );

            yield return exit;
        }
    }
}
