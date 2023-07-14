using System.Collections.Generic;
using Unity.VisualScripting;
using Microsoft.SemanticKernel.SemanticFunctions;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Semantic Functions")]
    [UnitTitle("Prompt Template Config")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class PromptTemplateConfigNew : Unit
    {
        [DoNotSerialize]
        public ValueInput description;

        [DoNotSerialize]
        public ValueInput completion;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput inputParameters;
        
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        protected override void Definition()
        {
            description = ValueInput<string>("description", string.Empty);
            completion = ValueInput<PromptTemplateConfig.CompletionConfig>("completion", new PromptTemplateConfig.CompletionConfig());
            inputParameters = ValueInput<List<PromptTemplateConfig.InputParameter>>("inputParameters", null);

            output = ValueOutput<PromptTemplateConfig>("output", (flow) =>
            {
                var descriptionValue = flow.GetValue<string>(description);
                var completionValue = flow.GetValue<PromptTemplateConfig.CompletionConfig>(completion);
                var inputParametersValue = inputParameters.hasValidConnection ? flow.GetValue<List<PromptTemplateConfig.InputParameter>>(inputParameters) : new();

                var promptTemplateConfig = new PromptTemplateConfig
                {
                    Description = descriptionValue,
                    Completion = completionValue,
                    Input = new PromptTemplateConfig.InputConfig
                    {
                        Parameters = inputParametersValue
                    }
                };
                return promptTemplateConfig;
            });
        }
    }
}
