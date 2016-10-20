namespace TPT_MMAS.Shared.Common.TPT
{
    public class NavigationParameterArgs
    {
        public ApiSettings Settings { get; set; }
        public object Parameter { get; set; }

        public NavigationParameterArgs(ApiSettings settings, object parameter)
        {
            Settings = settings;
            Parameter = parameter;
        }
    }
}
