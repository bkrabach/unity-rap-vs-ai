using Unity.VisualScripting;
using Microsoft.SemanticKernel.Orchestration;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Orchestration")]
    [UnitTitle("Create Context Variables")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class ContextVariablesNew : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        protected override void Definition()
        {
            output = ValueOutput<ContextVariables>("output", (flow) =>
            {
                var context = new ContextVariables();
                return context;
            });
        }
    }
}
