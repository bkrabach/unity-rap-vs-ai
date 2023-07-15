using Unity.VisualScripting;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel")]
    [UnitTitle("Kernel Builder Build")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class KernelBuilderBuild : Unit
    {
        [DoNotSerialize]
        public ValueInput kernelBuilder;

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
            kernelBuilder = ValueInput<KernelBuilder>("kernelBuilder");
            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<IKernel>("output", (flow) =>
            {
                var kernelBuilderValue = flow.GetValue<KernelBuilder>(kernelBuilder);
                var kernel = kernelBuilderValue.Build();
                return kernel;
            });

            Requirement(kernelBuilder, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
