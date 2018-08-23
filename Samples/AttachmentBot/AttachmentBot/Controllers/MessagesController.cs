using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AttachmentBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {

                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                Activity reply = activity.CreateReply("Hero cards with images");

                List<string> urls = new List<string>();
                urls.Add("https://www.python.org/static/img/python-logo@2x.png");
                urls.Add("https://www.r-project.org/Rlogo.png");

                reply.Attachments = new List<Attachment>();

                // create our actions
                var actions = new List<CardAction>();
                string[] choices = new string[3] { "I agree", "I disagree", "I'm neutral" };

                // some actions (with placeholder values)
                for (int i = 0; i < 3; i++)
                {
                    actions.Add(new CardAction
                    {
                        Title = choices[i], // shows up on card
                        Value = $"Action:{i}",
                        Type = ActionTypes.ImBack
                    });
                }

                // an action to link out to URL
                actions.Add(new CardAction
                {
                    Title = "R vs. Python Infographic", // shows up on card
                    Value = "https://www.datacamp.com/community/tutorials/r-or-python-for-data-analysis",
                    Type = ActionTypes.OpenUrl
                });

                // create a layout for all items in the attachment object (List or Carousel)
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                //  Add to attachment, our card with new card images and buttons
                for (int i = 0; i < 2; i++)
                {
                    reply.Attachments.Add(
                         new ThumbnailCard
                         {
                             Title = "A great language is...",
                             Images = new List<CardImage>
                            {
                                new CardImage
                                {
                                    Url = urls[i]
                                }
                            },
                             Buttons = actions
                         }.ToAttachment()
                    );
                }

                // reply
                await connector.Conversations.SendToConversationAsync(reply);

            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
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