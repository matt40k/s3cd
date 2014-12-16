namespace Solus3configdiscovery
{
    public class Entry
    {
        public static void Main(string[] args)
        {
            var output = new Output();
            var settings = new Settings();

            output.WriteTitle("School SOLUS3 Deployment Details");
            output.WriteBlankLine(2);

            // 1. SOLUS3 TAB
            output.WriteTitle("1. SOLUS3 TAB");
            output.WriteBlankLine(1);
            output.Write("School Details");
            output.Write("Name:     " + settings.GetSchoolName);
            output.Write("DfE code: " + settings.GetSchoolCode);
            output.Write("Postcode: " + settings.GetSchoolPostCode);
            output.WriteBlankLine(1);
            output.Write("Deployment Service Host Computer Name: " + settings.GetComputerName);
            output.WriteBlankLine(1);
            output.Write("Proxy Server Details");
            output.Write("Address:   " + settings.GetProxyAddress);
            output.Write("Port:      " + settings.GetProxyPort);
            output.Write("Domain:    ");
            output.Write("User Name: ");
            output.Write("Password:	 ");
            output.WriteBlankLine(1);

            // 2. UPDATES TAB
            output.WriteTitle("2. UPDATES TAB");
            output.Write("Update Repository: " + settings.GetUpdateRepository);
            output.WriteBlankLine(1);

            // 3. ALERTS TAB
            output.WriteTitle("3. ALERTS TAB");
            output.Write("SMTP Server:  <UNKNOWN>");
            output.Write("SMTP Port:    25");
            output.Write("User Name:    ");
            output.Write("Password:     ");
            output.Write("Sender Email: s3alert@<UNKNOWN>");
            output.WriteBlankLine(1);

            // 4. DEPLOYMENT ENVIRONMENT SETUP
            output.WriteTitle("4. DEPLOYMENT ENVIRONMENT SETUP");
            output.Write("SIMS .net");
            output.Write("SIMS database name:           " + settings.GetDatabaseName);
            output.Write("SIMS instance name:           " + settings.GetInstanceName);
            output.Write("SIMS database username:       sa");
            output.Write("SIMS database password:       <UNKNOWN>");

            output.Write("SQL Server BINN folder:       " + settings.GetSimsSqlBinn);

            output.Write("File Server Location:         " + settings.GetSimsShare);

            output.Write("DMS Document Server Location: " + settings.GetDmsDirectory);
            output.WriteBlankLine(1);

            output.Write("FMS");
            output.Write("FMS database name:            " + settings.GetFmsDatabaseName);
            output.Write("FMS instance name:            " + settings.GetFmsInstanceName);
            output.Write("FMS database username:        sa");
            output.Write("FMS database password:        <UNKNOWN>");
            output.Write("SQL Server BINN folder:       " + settings.GetFmsSqlBinn);

            /* TO-DO
             * =====
             * Need to add support for FDS
             *  
             * Most Academy's are now running two databases.
             * An LA and a new Academy FMS. Just another .INI file to read :)
             */
            output.WriteBlankLine(1);

            output.Write("SIMS Discover");
            output.Write("Installation Path:           " + settings.GetDiscoverServiceDirectory);
            output.WriteBlankLine(1);

            // 5. WORKSTATION
            output.WriteTitle("5. WORKSTATION");

            output.Write("Default sims.ini file location:       " + settings.GetUpdateRepository + "\\sims.ini");
            output.Write("Default connect.ini file location:    " + settings.GetUpdateRepository + "\\connect.ini");
            output.Write("Default fmsconnect.ini file location: " + settings.GetUpdateRepository + "\\fmsconnect.ini");

            output.WriteBlankLine(1);

            // 6. DEPLOYMENT ENVIONMENT SETUP
            output.WriteTitle("6. DEPLOYMENT ENVIRONMENT SETUP");

            output.WriteBlankLine(1);


            // Notes
            output.WriteBlankLine(1);
            output.WriteTitle("NOTES:");
            output.Write("Generated: " + settings.GetDateNow);
            output.WriteBlankLine(1);
            output.Write(settings.GetNotes);
        }
    }
}