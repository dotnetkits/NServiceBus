﻿namespace LicenseInstaller
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using Microsoft.Win32;
    using NDesk.Options;

    class Program
    {
        static string licensePath;
        static bool useHKLM;

        [STAThread]
        static int Main(string[] args)
        {
            if (!TryParseOptions(args))
            {
                return 0;
            }

            if (licensePath == null)
            {
                using (var openDialog = new OpenFileDialog())
                {
                    openDialog.Filter = "License files (*.xml)|*.xml|All files (*.*)|*.*";
                    openDialog.Title = "Select License file";

                    if (openDialog.ShowDialog() == DialogResult.OK)
                    {
                        licensePath = openDialog.FileName;
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(licensePath))
            {
                Console.Out.WriteLine("License file not installed.");
                return 1;
            }

            string selectedLicenseText = ReadAllTextWithoutLocking(licensePath);

            var rootKey = Registry.CurrentUser;
            if (useHKLM)
            {
                rootKey = Registry.LocalMachine;
            }

            using (var registryKey = rootKey.CreateSubKey(String.Format(@"SOFTWARE\NServiceBus\{0}", GetNServiceBusVersion().ToString(2))))
            {
                if (registryKey == null)
                {
                    Console.Out.WriteLine("License file not installed.");
                    return 1;
                }
                registryKey.SetValue("License", selectedLicenseText, RegistryValueKind.String);
            }

            Console.Out.WriteLine("License file installed.");

            return 0;
        }

        static bool TryParseOptions(IEnumerable<string> args)
        {
            OptionSet optionSet = null;
            Func<bool> action = () => true;

            optionSet = new OptionSet
                {
                    {
                        "machine-wide|m",
                        @"Installs license in HKEY_LOCAL_MACHINE\SOFTWARE\NServiceBus, by default if not specified the license is installed in HKEY_CURRENT_USER\SOFTWARE\NServiceBus"
                        , s => action = () =>
                            {
                                useHKLM = true;
                                return true;
                            }
                    },
                    {
                        "help|h|?", "Help about the command line options", key => action = () =>
                            {
                                PrintUsage(optionSet);
                                return false;
                            }
                    },
                };

            try
            {
                var unparsedArgs = optionSet.Parse(args);

                if (unparsedArgs.Count > 0)
                {
                    licensePath = unparsedArgs[0];
                }
                
                return action();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PrintUsage(optionSet);
            }

            return false;
        }

        static void PrintUsage(OptionSet optionSet)
        {
            Console.WriteLine(
                @"
NServiceBus license installer
-------------------------------------------
Copyright (C) 2010 - {0} - NServiceBus Ltd.
-------------------------------------------
Command line options:", DateTime.Now.Year);

            optionSet.WriteOptionDescriptions(Console.Out);

            Console.Out.WriteLine();
        }

        static string ReadAllTextWithoutLocking(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                return textReader.ReadToEnd();
            }
        }

        static Version GetNServiceBusVersion()
        {
            var assembyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            return new Version(assembyVersion.Major, assembyVersion.Minor);
        }
    }
}
