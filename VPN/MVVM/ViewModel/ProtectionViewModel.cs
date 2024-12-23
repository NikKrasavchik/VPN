﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VPN.Core;

namespace VPN.MVVM.ViewModel
{
    internal class ProtectionViewModel : ObservableObject
    {
        public ObservableCollection<ServerModel> Servers { get; set; }

        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        private string _connectionStatus;
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set 
            { 
                _connectionStatus = value;
                OnPropertyChanged();
            } 
        }
        public RelayCommand ConnectCommand { get; set; }
        public ProtectionViewModel()
        {
            Servers = new ObservableCollection<ServerModel>();
            for (int i = 0; i < 1; i++)
            {
                Servers.Add(new ServerModel()
                {
                    Country = "USA"
                });
            }

            ConnectCommand = new RelayCommand(o => 
            {
                Task.Run(() =>
                {
                    ConnectionStatus = "Connecting..";
                    var process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                    process.StartInfo.ArgumentList.Add(@"/c rasdial MyServer vpnbook c28hes5 /phonebook:UK68.vpnbook.com.pbk");

                    process.Start();
                    process.WaitForExit();

                    switch (process.ExitCode)
                    {
                        case 0:
                            Debug.WriteLine("Success!");
                            ConnectionStatus = "Connection successful!";
                            break;

                        case 691:
                            Debug.WriteLine("Wrong credential!");
                            ConnectionStatus = "Offline";
                            break;

                        default:
                            Debug.WriteLine($"Error: {process.ExitCode}");
                            ConnectionStatus = "Offline";
                            break;
                    }
                });
            });
            ServerBuilder();
        }

        private void ServerBuilder()
        {
            var address = "UK68.vpnbook.com";
            var FolderPath = $"{Directory.GetCurrentDirectory()}/VPN";
            var PbkPath = $"{FolderPath}/{address}.pbk";

            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
            
            if (File.Exists(FolderPath))
            {
                MessageBox.Show("Connection already axists!");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("[MyServer]");
            sb.AppendLine("MEDIA=rastapi");
            sb.AppendLine("Port=VPN2-0");
            sb.AppendLine("Device=WAN Miniport (IKEv2)");
            sb.AppendLine("DEVICE=vpn");
            sb.AppendLine($"PhoneNumber={address}");
            File.WriteAllText(PbkPath, sb.ToString());
        }
    }
}
