namespace BookShop.Data
{
    internal class Configuration
    {
        // insert your server 
        internal static string ConnectionString => 
            @"Server=.;Database=BookShop;Integrated Security=True;";
    }
}
