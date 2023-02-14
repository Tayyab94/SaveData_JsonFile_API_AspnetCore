using MessageApp.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageApp.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessage(Message data);
        IEnumerable<Message> ReceivedMessages(string receiverId, bool isDeleted);
    }
}
