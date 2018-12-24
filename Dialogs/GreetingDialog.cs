using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MultiDialogsBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.PostAsync("Hi I'm a Bot");
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;            
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var userName = String.Empty;
            var getName = false;

            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
            }
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name");
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync(String.Format("Hi {0}. Please share youtube url where you want to comment.", userName));
            }
            context.Wait(MessageReceivedAsync);
        }
        
    }
}