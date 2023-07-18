using Microsoft.SemanticKernel;
using Unity.VisualScripting;

namespace Scripts.SemanticKernel.SemanticFunctions
{
    [UnitCategory("Semantic Kernel")]
    [UnitTitle("New Kernel Builder")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class NewKernelBuilder : Unit
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

            output = ValueOutput<KernelBuilder>("output", (flow) =>
            {
                var kernelBuilder = new KernelBuilder();
                return kernelBuilder;
            });

            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
