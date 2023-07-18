using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Unity.VisualScripting;

namespace Scripts.SemanticKernel.AI.ChatCompletion
{
    [UnitCategory("Semantic Kernel/AI/Chat Completion")]
    [UnitTitle("Chat Completion")]
    [UnitSubtitle("Get chat completion service")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class GetServiceChatCompletion : Unit
    {
        [DoNotSerialize]
        public ValueInput kernel;

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

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<IChatCompletion>("output", (flow) =>
            {
                var kernelValue = flow.GetValue<IKernel>(kernel);

                var chatService = kernelValue.GetService<IChatCompletion>();
                return chatService;
            });

            Requirement(kernel, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
