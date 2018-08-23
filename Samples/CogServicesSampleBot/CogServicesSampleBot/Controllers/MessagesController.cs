using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

using CogServicesSampleBot.Utilities;

namespace CogServicesSampleBot
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

                var responseString = "";

                /** COGNITIVE SERVICE API CALL CODE SNIPPETS */

                // One would probably want to drive these code samples with menus to
                //    prevent using more API calls than necessary


                //  Speaker Recognition API sample
                if (activity.Text == "create a speaker id")
                {

                    // Speaker Recognition API (part of Speech)
                    //  1.  Create an identification profile

                    var client = new HttpClient();

                    // Put together the request body string (dictionary -> json -> string)
                    var localeLang = "en-US"; //language
                    Dictionary<string, string> requestDict = new Dictionary<string, string>();
                    requestDict.Add("locale", localeLang);

                    string json = JsonConvert.SerializeObject(requestDict);

                    // Still need to encode an empty string for POST
                    var queryString = HttpUtility.ParseQueryString(string.Empty);

                    // Request headers - key is placed here
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Keys.SpeakerRecogSubscriptionKey);

                    var uri = "https://api.projectoxford.ai/spid/v1.0/identificationProfiles?" + queryString;

                    // Request body
                    byte[] byteData = Encoding.UTF8.GetBytes(json);

                    using (var content = new ByteArrayContent(byteData))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        var serviceResponse = await client.PostAsync(uri, content);

                        // Convert the API call response to string for bot to reply
                        responseString = serviceResponse.Content.ReadAsStringAsync().Result;


                    }
                } // end if activity equals "create speaker id"

                /*-----------------------------START HERE--------------------------------*/

                // Next API...
                else if (activity.Text == "next api command...")
                {
                    // call api and return a response (string)
                }

                else
                {
                    responseString = "That command was not recognized.  Please try again.  " +
                        "Your choices are: 'create a speaker id', ...";
                }

                // return our reply to the user through the bot connector
                Activity reply = activity.CreateReply(responseString);
                await connector.Conversations.ReplyToActivityAsync(reply);
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
