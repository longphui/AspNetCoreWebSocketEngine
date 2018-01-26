﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Core.SocketClient
{
    public class SocketClientMgr
    {
        private static object o = new object();

        public static SocketClientMgr Instance
        {
            get
            {
                lock (o)
                {
                    if (_instance == null)
                    {
                        _instance = new SocketClientMgr();
                    }
                    return _instance;
                }
            }
        }

        private static SocketClientMgr _instance = null;

        private Dictionary<string, SocketClient> _sessionclients;
        private Dictionary<string, SocketClient> _accclients;

        private SocketClientMgr()
        {
            _sessionclients = new Dictionary<string, SocketClient>();
            _accclients = new Dictionary<string, SocketClient>();
        }

        public SocketClient GetClient(string sessionid)
        {
            return _sessionclients.ContainsKey(sessionid) ? _sessionclients[sessionid] : null;
        }

        public void AddClient(SocketClient client)
        {
            _sessionclients.Add(client.SessionId, client);
        }

        public async Task SendAll(string msg)
        {
            foreach (var client in _sessionclients.Values)
            {
                await client.SendMsg(msg);
            }
        }

        public async Task SendTo(string sessionid, string msg)
        {
            var client = _sessionclients[sessionid];
            await client.SendMsg(msg);

        }

        public void Remove(string eArg1)
        {
            var client = _sessionclients[eArg1];
            _sessionclients.Remove(eArg1);

        }

        public async Task CloseAsync(string sessionId)
        {
            var client = _sessionclients[sessionId];
            _sessionclients.Remove(sessionId);
            await client.Close();

        }

        internal List<SocketClient> GetAllClients()
        {
            return _sessionclients.Values.ToList();
        }
    }
}
