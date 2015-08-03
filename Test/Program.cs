using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Modbus;
using System.IO.Ports;

namespace Test
{
    class Program
    {

        #region Test functions

        #region Modbus RTU slave

        /// <summary>
        /// Test a modbus RTU slave
        /// </summary>
        static void Test_ModbusRTUSlave()
        {
            byte unit_id = 1;
            // Created datastore for unit ID 1
            Datastore ds = new Datastore(unit_id);
            // Crete instance of modbus serial RTU (replace COMx with a free serial port - ex. COM5)
            ModbusSlaveSerial ms = new ModbusSlaveSerial(new Datastore[] { ds }, ModbusSerialType.RTU, "COM7", 19200, 8, Parity.Even, StopBits.One, Handshake.None);
            // Start listen
            ms.StartListen();
            // Print and write some registers...
            Random rnd = new Random();
            while (true)
            {
                Console.Write(
                    "---------------------- READING ----------------------" + Environment.NewLine +
                    "Input register n.1 : " + ms.ModbusDB.Single(x => x.UnitID == unit_id).InputRegisters[30001].ToString("D5") + Environment.NewLine +
                    "Input register n.1 : " + ms.ModbusDB.Single(x => x.UnitID == unit_id).InputRegisters[30002].ToString("D5") + Environment.NewLine +
                    "Input register n.2 : " + ms.ModbusDB.Single(x => x.UnitID == unit_id).InputRegisters[30003].ToString("D5") + Environment.NewLine);
                    //"Coil register    n.32 : " + ms.ModbusDB.Single(x => x.UnitID == unit_id).Coils[31].ToString() + Environment.NewLine +
                    //"---------------------- WRITING ----------------------" + Environment.NewLine);
                //ms.ModbusDB.Single(x => x.UnitID == unit_id).HoldingRegisters[1] = (ushort)rnd.Next(ushort.MinValue, ushort.MaxValue);
                //Console.WriteLine(
                //    "Holding register n.2  : " + ms.ModbusDB.Single(x => x.UnitID == unit_id).HoldingRegisters[1].ToString("D5"));
                //ms.ModbusDB.Single(x => x.UnitID == unit_id).Coils[15] = Convert.ToBoolean(rnd.Next(0, 1));
                //Console.WriteLine(
                //    "Coil register    n.16 : " + ms.ModbusDB.Single(x => x.UnitID == unit_id).Coils[15].ToString());
                // Exec the cicle each 2 seconds
                Thread.Sleep(2000);
            }
        }

        #endregion

        #region Modbus RTU master

        /// <summary>
        /// Test modbus RTU master function on a slave RTU id = 5
        /// </summary>
        static void Test_ModbusRTUMaster(ModbusMasterSerial mm)
        {
            try
            { 
                Console.Write(
                "---------------------- READING ----------------------" + Environment.NewLine +
                "Input register no. 30501 (Meas. 1) : " + mm.ReadInputRegisters(65, 30501, 1).First().ToString("D5") + Environment.NewLine +
                "Input register no. 30502 (Meas. 2) : " + mm.ReadInputRegisters(65, 30502, 1).First().ToString("D5") + Environment.NewLine + Environment.NewLine);

            }

               catch(Exception ex)
            {
                Console.Write(Environment.NewLine + ex.Message + Environment.NewLine);

                Thread.Sleep(5000);

            }
            
          }

        #endregion

        #endregion

        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ModbusMasterSerial mm = new ModbusMasterSerial(ModbusSerialType.RTU, "COM7", 19200, 8, Parity.Even, StopBits.One, Handshake.None);

            mm.Connect();

            while(true)
            {
                Test_ModbusRTUMaster(mm);

                Thread.Sleep(1000);
            }
            //Test_ModbusRTUSlave();
        }
    }
}
