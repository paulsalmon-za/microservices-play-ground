namespace Abstractions
{
    public class SubscriptionRegistrationBuilder
    {
        private SubscriptionRegistration _build;

        public SubscriptionRegistrationBuilder(IInformationNameResolver resolver, SubscriptionRegistration registration)
        {
            _build = registration;
        }

        public SubscriptionRegistrationBuilder CommandOf<T>()
            where T : ICommand
        {

            _build.SubscribeCommand<T>();
            return this;
        }

        public SubscriptionRegistrationBuilder ResultOf<T>()
            where T : IResult
        {

            _build.SubscribeResult<T>();
            return this;
        }

    }
}