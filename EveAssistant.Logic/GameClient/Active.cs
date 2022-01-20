using System.Collections.Generic;
using System.Diagnostics;

namespace EveAssistant.Logic.GameClient
{
    public static class Active
    {
        public static List<Client> GetList(string clientTitle)
        {
            var list = new List<Client>();


            foreach (var pList in Process.GetProcesses())
            {
                if (!pList.MainWindowTitle.Contains(clientTitle)) continue;

                var hWnd = pList.MainWindowHandle;
                var name = pList.MainWindowTitle.Replace(clientTitle, "");

                list.Add(new Client(name, hWnd));
            }

            return list;
        }
    }
}