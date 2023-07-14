using System.Collections.Generic;
using Unity.VisualScripting;
using Microsoft.SemanticKernel.SemanticFunctions;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Semantic Functions")]
    [UnitTitle("Completion Config")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class PromptTemplateConfigCompletionConfigNew : Unit
    {
        [DoNotSerialize]
        [AllowsNull]
        public ValueInput maxTokens;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput temperature;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput topP;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput presencePenalty;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput frequencyPenalty;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput stopSequences;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        protected override void Definition()
        {
            maxTokens = ValueInput<int>("maxTokens", 256);
            temperature = ValueInput<float>("temperature", 0);
            topP = ValueInput<float>("topP", 0);
            presencePenalty = ValueInput<float>("presencePenalty", 0);
            frequencyPenalty = ValueInput<float>("frequencyPenalty", 0);
            stopSequences = ValueInput<List<string>>("stopSequences", null);

            output = ValueOutput<PromptTemplateConfig.CompletionConfig>("output", (flow) =>
            {
                var maxTokensValue = flow.GetValue<int>(maxTokens);
                var temperatureValue = flow.GetValue<float>(temperature);
                var topPValue = flow.GetValue<float>(topP);
                var presencePenaltyValue = flow.GetValue<float>(presencePenalty);
                var frequencyPenaltyValue = flow.GetValue<float>(frequencyPenalty);
                var stopSequencesValue = stopSequences.hasValidConnection ? flow.GetValue<List<string>>(stopSequences) : null;

                var completionConfig = new PromptTemplateConfig.CompletionConfig
                {
                    MaxTokens = maxTokensValue,
                    Temperature = temperatureValue,
                    TopP = topPValue,
                    FrequencyPenalty = frequencyPenaltyValue,
                    PresencePenalty = presencePenaltyValue,
                    StopSequences = stopSequencesValue,
                };
                return completionConfig;
            });
        }
    }
}
