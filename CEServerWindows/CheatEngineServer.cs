using CEServerWindows.CheatEnginePackets;
using System;
using System.Net;
using System.Net.Sockets;
using CEServerWindows.CheatEnginePackets.C2S;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace CEServerWindows
{
    public class CheatEngineServer : IDisposable
    {
        private TcpListener _tcpListener;
        private PacketManager _packetManager;
        private CancellationTokenSource _tokenSource;
#pragma warning disable CS0414
        private bool _listening;
#pragma warning restore CS0414
        private CancellationToken _token;
        private Mode _mode = Mode.FPGA;
        public bool enableWPM = false;
        public static CheatEngineServer instance;

        public CheatEngineServer(ushort port = 52736) : this(port, new PacketManager())
        {
            instance = this;
            if (_mode == Mode.FPGA)
            {
                new FPGA();
            }
            this.RegisterDefaultHandlers();
        }

        public CheatEngineServer(PacketManager pm) : this(52736, pm)
        {

        }

        public CheatEngineServer(ushort port, PacketManager pm)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            this._packetManager = pm;
        }

        private void HandleReceivedClient(TcpClient client)
        {
            var clientStream = client.GetStream();
            var reader = new BinaryReader(clientStream);
            var writer = new BinaryWriter(clientStream);
            while (true)
            {
                try
                {
                    var command = this._packetManager.ReadNextCommand(reader);
                    var output = this._packetManager.ProcessAndGetBytes(command);

                    //Console.WriteLine($"{command.CommandType}");
                    
                    /* if(command.CommandType != CommandType.CMD_READPROCESSMEMORY)
                         Console.WriteLine(BitConverter.ToString(output).Replace("-", ""));*/
                   // Console.WriteLine("{0} returned {1} bytes", command.CommandType, output.Length);
                    writer.Write(output);
                    writer.Flush();
                    //   Handle(stream, writer, cmd);

                }
                catch(EndOfStreamException)
                {
                    client.Close();
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e + ": "+  e.Message);
                    Console.WriteLine(e.StackTrace);
                    client.Close();
                    break;
                } 
            }
        }

        public async Task StartAsync(CancellationToken? token = null)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token ?? new CancellationToken());
            _token = _tokenSource.Token;
            _tcpListener.Start();
            _listening = true;

            try
            {
                while (!_token.IsCancellationRequested)
                {
                    var tcpClientTask = _tcpListener.AcceptTcpClientAsync();
                    var result = await tcpClientTask;
                    Console.WriteLine("New client");
                    _ = Task.Run(() =>
                      {
                          HandleReceivedClient(result);
                      }, _token);
                }
            }
            finally
            {
                _tcpListener.Stop();
                _listening = false;
            }
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
        }

        public void Dispose()
        {
            Stop();
        }

        private void RegisterDefaultHandlers()
        {
            if (_mode == Mode.FPGA)
            {
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.CreateToolHelp32SnapshotCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.GetVersionCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.Module32FirstCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.Module32NextCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.Process32FirstCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.Process32NextCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.CloseHandleCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.OpenProcessCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.GetArchitectureCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.VirtualQueryExCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.VirtualQueryExFullCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.ReadProcessMemoryCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.GetSymbolsFromFileCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.GetRegionInfoCommand());
                if(enableWPM) this.RegisterCommandHandler(new CheatEnginePackets.C2S.FPGA.WriteProcessMemoryCommand());
            }
            else if(_mode == Mode.x64)
            {
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.CreateToolHelp32SnapshotCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.GetVersionCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.Module32FirstCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.Module32NextCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.Process32FirstCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.Process32NextCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.CloseHandleCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.OpenProcessCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.GetArchitectureCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.VirtualQueryExCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.VirtualQueryExFullCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.ReadProcessMemoryCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.GetSymbolsFromFileCommand());
                this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.GetRegionInfoCommand());
                if(enableWPM) this.RegisterCommandHandler(new CheatEnginePackets.C2S.WIN.WriteProcessMemoryCommand());
            }
        }

        public void RegisterCommandHandler(ICheatEngineCommand command)
        {
            this._packetManager.RegisterCommand(command);
        }

    }
}
