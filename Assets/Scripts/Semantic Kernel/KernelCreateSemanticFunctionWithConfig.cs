using Unity.VisualScripting;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SemanticFunctions;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel")]
    [UnitTitle("Create Semantic Function (kernel, promptTemplate, config)")]
    [UnitShortTitle("Create Semantic Function")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class KernelCreateSemanticFunctionWithConfig : Unit
    {
        [DoNotSerialize]
        public ValueInput kernel;

        [DoNotSerialize]
        public ValueInput promptTemplate;

        [DoNotSerialize]
        public ValueInput config;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput functionName;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput skillName;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput enter;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput exit;

        protected override void Definition()
        {
            kernel = ValueInput<IKernel>("kernel");
            promptTemplate = ValueInput<string>("promptTemplate");
            config = ValueInput<PromptTemplateConfig>("config");
            functionName = ValueInput<string>("functionName", null);
            skillName = ValueInput<string>("skillName", null);

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<ISKFunction>("output", (flow) =>
            {
                var kernelValue = flow.GetValue<IKernel>(kernel);
                var promptTemplateValue = flow.GetValue<string>(promptTemplate);
                var configValue = flow.GetValue<PromptTemplateConfig>(config);
                var functionNameValue = flow.GetValue<string>(functionName);
                var skillNameValue = flow.GetValue<string>(skillName);

                return kernelValue.CreateSemanticFunction(
                    promptTemplateValue,
                    configValue,
                    functionNameValue,
                    skillNameValue
                );
            });

            Requirement(kernel, enter);
            Requirement(promptTemplate, enter);
            Requirement(config, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
