using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;

namespace EchoTestBot
{

    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync("You said: " + message.Text);
            context.Wait(MessageReceivedAsync);
        }
    }

    //Uncomment the following EchoDialog class (and comment the previous) to add
    //    reset capabilites(add state in the form of a variable called count)

    //[Serializable]
    //public class EchoDialog : IDialog<object>
    //{
    //    // the state we are persisting with this dialog on each message
    //    protected int count = 1;


    //    public async Task StartAsync(IDialogContext context)
    //    {
    //        context.Wait(MessageReceivedAsync);
    //    }

    //    // we have added check to see if the input was "reset"
    //    // if that is true we use the built-in Prompts.Confirm dialog to spawn a sub-dialog
    //    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    //    {
    //        var message = await argument;
    //        if (message.Text == "reset")
    //        {
    //            // sub-dialog
    //            PromptDialog.Confirm(
    //                context,
    //                AfterResetAsync, // confirm then pass onto the AfterResetAync method
    //                "Are you sure you want to reset the count?",
    //                "Didn't get that!",
    //                promptStyle: PromptStyle.None);
    //        }
    //        else
    //        {
    //            await context.PostAsync(string.Format("{0}: You said: {1}", this.count++, message.Text));
    //            context.Wait(MessageReceivedAsync);
    //        }
    //    }

    //    // check on the response and perform the action including 
    //    //    sending a message back to the user
    //    public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
    //    {
    //        var confirm = await argument;
    //        if (confirm)
    //        {
    //            this.count = 1;
    //            await context.PostAsync("Count reset.");
    //        }
    //        else
    //        {
    //            await context.PostAsync("Did not reset count.");
    //        }

    //        // final step is to do IDialogContext.Wait with 
    //        //     continuation back to MessageReceivedAsync on the next message
    //        context.Wait(MessageReceivedAsync);
    //    }
    //}

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 

        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new EchoDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}