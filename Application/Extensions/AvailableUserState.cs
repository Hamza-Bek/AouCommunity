using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public class AvailableUserState
    {
        public string ReceiverId { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;

        public void SetStates(string fullname , string receiverId)
        {
            Fullname = fullname;
            ReceiverId = receiverId;
        }
    }
}
