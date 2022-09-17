﻿using AlbionDamageMetter.Albion;
using PacketDotNet;
using PhotonPackageParser;
using SharpPcap;

namespace AlbionDamageMetter.Services
{
    public class CaptureDeviceNetwork : IHostedService
    {
        private readonly List<ICaptureDevice> _capturedDevices = new();
        private readonly ILogger<CaptureDeviceNetwork> _logger;
        private readonly PhotonParser _receiver;

        public CaptureDeviceNetwork(ILogger<CaptureDeviceNetwork> logger)
        {
            _capturedDevices.AddRange(CaptureDeviceList.Instance);
            _logger = logger;
            _receiver = new AlbionPackageParser();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var device in _capturedDevices)
            {
                _logger.LogInformation(device.Name);
                if (device.Started) continue;
                device.Open(new DeviceConfiguration()
                {
                    Mode = DeviceModes.DataTransferUdp,
                    ReadTimeout = 5000
                });
                device.OnPacketArrival += DeviceOnPacketArrival;
                device.StartCapture();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private void DeviceOnPacketArrival(object sender, PacketCapture e)
        {
            var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data).Extract<UdpPacket>();
            if (packet != null && (packet.SourcePort == 5056 || packet.DestinationPort == 5056))
                _receiver.ReceivePacket(packet.PayloadData);
        }
    }
}
