using Unity.VisualScripting;
using Microsoft.SemanticKernel;

namespace Scripts.SemanticKernel.SemanticFunctions
{
    [UnitCategory("Semantic Kernel")]
    [UnitTitle("Create Kernel Instance")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class KernelBuilderCreate : Unit
    {
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
            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<IKernel>("output", (flow) =>
            {
                var kernel = KernelBuilder.Create();
                return kernel;
            });

            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
