using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;
using Unity.VisualScripting;

namespace Scripts.SemanticKernel.AI.ChatCompletion
{
    [UnitCategory("Semantic Kernel/AI/Chat Completion")]
    [UnitTitle("Add User Message")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class AddUserMessage : Unit
    {
        [DoNotSerialize]
        public ValueInput chat;

        [DoNotSerialize]
        public ValueInput userMessage;

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
            chat = ValueInput<OpenAIChatHistory>("chat");
            userMessage = ValueInput<string>("userMessage");

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<OpenAIChatHistory>("output", (flow) =>
            {
                var chatValue = flow.GetValue<OpenAIChatHistory>(chat);
                var userMessageValue = flow.GetValue<string>(userMessage);

                chatValue.AddUserMessage(userMessageValue);

                return chatValue;
            });

            Requirement(chat, enter);
            Requirement(userMessage, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
