using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    [ServiceContract]
    public interface DataServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        [FaultContract(typeof(ErrorData))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string firstName, out string lastName, out byte[] profileImage);

    }

    [DataContract]
    public class ErrorData
    {
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
