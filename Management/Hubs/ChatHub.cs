
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        
        public Task SendMessageToGroup(string groupName, string username, string message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveGroupMessage", groupName, username, message);
        }
        static string _server_connect = "";
        static List<string> connect_list = new List<string>();
        static List<string> name_list = new List<string>();
        public async Task AddGroup(string groupName, string username)
        {
            if (connect_list.Count == 0)
            {
                connect_list.Add("");
                name_list.Add("管理員");
            }
            if (_server_connect == ""&&username.Contains("管理員"))
            {
                _server_connect = Context.ConnectionId;
                if (connect_list.Count == 0)
                {
                    connect_list.Add(Context.ConnectionId);
                    name_list.Add("管理員");
                }
                else
                {
                    connect_list[0] = Context.ConnectionId;
                    
                }
            }
            else {

                int count_same = 0;
                foreach (var item in connect_list)
                {
                    if(item == Context.ConnectionId)
                    {
                        
                        count_same++;

                    }
                }
                if(count_same == 0) { 
                    connect_list.Add(Context.ConnectionId);
                    name_list.Add(username);
                    
                }
                else
                {
                    name_list[count_same] = username;
                }
                 
            }
            string strnamelist = "";
            foreach (var item in name_list)
            {
                if (!item.Contains("管理員"))
                {
                    strnamelist += item+" , ";
                }
            }
            await Clients.Client(_server_connect).SendAsync("RecAddGroupMsg", "", strnamelist);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            
            await Clients.Group(groupName).SendAsync("RecAddGroupMsg", $"{username} 已加入 群組：{groupName}。",  "");
        }
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
        public async Task Removemanager(string groupName)
        {
            connect_list[0] = "";
            _server_connect = "";
            await Clients.Group(groupName).SendAsync("RecAddGroupMsg", $"客服已下線");
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendImgMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveImgMessage", user, message);            
        }
        public async Task SendPrivateMessage(string to_user, string message, string user)
        {
            if (_server_connect == "")
            {
                await Clients.Caller.SendAsync("ReceiveprivateMessage", to_user, "客服目前不在線上");
            }
            else
            {
                if (user.Contains("管理員"))
                {
                    int get_index = 0;
                    foreach (var item in name_list)
                    {

                        if (item == to_user)
                        {
                            break;
                        }
                        else
                        {
                            get_index++;
                        }
                    }
                    string _get_connectID = connect_list[get_index];
                    await Clients.Caller.SendAsync("ReceiveprivateMessage", user, message);
                    await Clients.Client(_get_connectID).SendAsync("ReceiveprivateMessage", user, message);
                }
                else
                {
                    string _get_connectID = connect_list[0];
                    await Clients.Caller.SendAsync("ReceiveprivateMessage", user, message);
                    await Clients.Client(_get_connectID).SendAsync("ReceiveprivateMessage", user, message);
                }
             
            }
        }
    }
}