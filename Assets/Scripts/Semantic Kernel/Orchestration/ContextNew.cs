using Unity.VisualScripting;
using Microsoft.SemanticKernel.Orchestration;

namespace Scripts.SemanticKernel.Orchestration
{
    [UnitCategory("Semantic Kernel/Orchestration")]
    [UnitTitle("Create Context Variables")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class ContextNew : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        protected override void Definition()
        {
            output = ValueOutput<SKContext>("output", (flow) =>
            {
                var context = new SKContext();
                return context;
            });
        }
    }
}
