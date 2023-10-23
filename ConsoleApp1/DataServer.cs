using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DatabaseDLL;

namespace ConsoleApp1
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : DataServerInterface
    {
        private DatabaseClass database;

        public DataServer()
        {
            database = new DatabaseClass();
        }

        public int GetNumEntries()
        {
            return database.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string firstName, out string lastName, out byte[] profileImage)
        {
            if (index >= 0 && index < database.GetNumEntries())
            {
                acctNo = database.GetAcctNoByIndex(index);
                pin = database.GetPINByIndex(index);
                bal = database.GetBalanceByIndex(index);
                firstName = database.GetFirstNameByIndex(index);
                lastName = database.GetLastNameByIndex(index);
                profileImage = database.GetProfileImageByIndex(index);
            }
            else
            {
                throw new ArgumentOutOfRangeException("index", "Index is out of range");
            }
        }
    }
}
