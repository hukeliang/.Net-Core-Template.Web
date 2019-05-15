using System;
using System.Collections.Generic;

namespace Common.Helper.Core.CSharpExpand
{
    /// <summary>
    /// 权重轮询算调度算法
    /// </summary>
    public class WeightedRoundRobin
    {
        /// <summary>
        /// 上次选择的服务器
        /// </summary>
        private int _currentIndex = -1;

        /// <summary>
        /// 当前调度的权值
        /// </summary>
        private int _currentWeight = 0;

        /// <summary>
        /// 最大权重
        /// </summary>
        private readonly int _maxWeight;

        /// <summary>
        /// 权重的最大公约数
        /// </summary>
        private readonly int _gcdWeight;

        /// <summary>
        /// 服务器数
        /// </summary>
        private readonly int _serverCount;


        /// <summary>
        /// 服务器列表
        /// </summary>
        private readonly List<Server> _servers;

        public WeightedRoundRobin(List<Server> value)
        {
            Console.Out.WriteLine("对象被构建");
            _servers = value;
            _maxWeight = GetMaxWeight(_servers);
            _gcdWeight = GreatestCommonDivisor(_servers);
            _serverCount = _servers.Count;

        }

        /// <summary>
        /// 得到两值的最大公约数
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public int GreaterCommonDivisor(int value1, int value2)
        {
            if (value1 % value2 == 0)
            {
                return value2;
            }

            return GreaterCommonDivisor(value2, value1 % value2);
        }


        /// <summary>
        /// 得到list中所有权重的最大公约数，实际上是两两取最大公约数d，然后得到的d与下一个权重取最大公约数，直至遍历完
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public int GreatestCommonDivisor(List<Server> serversList)
        {
            int divisor = 0;
            for (int index = 0; index < _serverCount - 1; index++)
            {
                if (index == 0)
                {
                    divisor = GreaterCommonDivisor(serversList[index].Weight, serversList[index + 1].Weight);
                }
                else
                {
                    divisor = GreaterCommonDivisor(divisor, serversList[index].Weight);
                }
            }

            return divisor;
        }

        /// <summary>
        /// 算法流程：  
        /// 假设有一组服务器 S = { S0, S1, …, Sn - 1 } 
        /// 有相应的权重，变量currentIndex表示上次选择的服务器
        /// 权值currentWeight初始化为0，currentIndex初始化为-1 ，当第一次的时候返回 权值取最大的那个服务器， 
        /// 通过权重的不断递减 寻找 适合的服务器返回
        /// </summary>
        /// <returns></returns>
        public Server GetServer()
        {
            while (true)
            {
                _currentIndex = (_currentIndex + 1) % _serverCount;
                if (_currentIndex == 0)
                {
                    _currentWeight = _currentWeight - _gcdWeight;
                    if (_currentWeight <= 0)
                    {
                        _currentWeight = _maxWeight;
                        if (_currentWeight == 0)
                        {
                            return null;
                        }
                    }
                }
                if (_servers[_currentIndex].Weight >= _currentWeight)
                {
                    return _servers[_currentIndex];
                }
            }
        }

        /// <summary>
        /// 获取最大的权值
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        private static int GetMaxWeight(List<Server> servers)
        {
            int max = 0;
            foreach (Server s in servers)
            {
                if (s.Weight > max)
                {
                    max = s.Weight;
                }
            }

            return max;
        }
    }

    /// <summary>
    /// 服务器结构
    /// </summary>
    public class Server
    {
        public string Address;
        public int Weight;
    }
}

