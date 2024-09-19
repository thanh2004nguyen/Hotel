using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Concurrent;
namespace YourNamespace.Hubs
{
    public class ChatHub : Hub
    {
        private readonly HotelDbContext _context;
        private static readonly ConcurrentDictionary<int, HashSet<string>> userConnections = new ConcurrentDictionary<int, HashSet<string>>();
        public ChatHub(HotelDbContext context)
        {
            _context = context;
        }
        public override Task OnConnectedAsync()
        {
            var senderIdClaim = Context.User?.FindFirst("id");
            if (senderIdClaim != null && int.TryParse(senderIdClaim.Value, out var senderId))
            {
                var connections = userConnections.GetOrAdd(senderId, _ => new HashSet<string>());
                lock (connections)
                {
                    connections.Add(Context.ConnectionId);
                }
                Console.WriteLine($"User {senderId} connected with connectionId {Context.ConnectionId}");
            }
            else
            {
                Console.WriteLine("SenderId not found or invalid on connection.");
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var senderIdClaim = Context.User?.FindFirst("id");
            if (senderIdClaim != null && int.TryParse(senderIdClaim.Value, out var senderId))
            {
                if (userConnections.TryGetValue(senderId, out var connections))
                {
                    lock (connections)
                    {
                        connections.Remove(Context.ConnectionId);
                        if (connections.Count == 0)
                        {
                            userConnections.TryRemove(senderId, out _);
                        }
                    }
                    Console.WriteLine($"User {senderId} disconnected from connectionId {Context.ConnectionId}");
                }
            }
            else
            {
                Console.WriteLine("SenderId not found or invalid on disconnection.");
            }

            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(int receiverId, string content)
        {
            var senderIdClaim = Context.User?.FindFirst("id");

            if (senderIdClaim == null || !int.TryParse(senderIdClaim.Value, out var senderId))
            {
                Console.WriteLine("SenderId not found or invalid.");
                return;
            }

            if (receiverId <= 0)
            {
                Console.WriteLine($"Invalid receiverId: {receiverId}");
                return;
            }

            Console.WriteLine($"Attempting to send message. SenderId: {senderId}, ReceiverId: {receiverId}, Content: {content}");

            var message = new Message
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = receiverId,
                DateSend = DateTime.Now
            };

            _context.Messages.Add(message);

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("Message saved to database.");
            }   
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message to database: {ex.Message}");
                return;
            }

            try
            {
                if (userConnections.TryGetValue(receiverId, out var connections))
                {
                    foreach (var connectionId in connections)
                    {
                        await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, content, message.DateSend);
                        Console.WriteLine($"Message sent to receiverId {receiverId} at connectionId {connectionId} at senderId {senderId} at content {content} at content {message.DateSend}");
                    }
                }
                else
                {
                    Console.WriteLine($"No active connections for receiverId {receiverId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message via SignalR: {ex.Message}");
            }

        }


    }
}
