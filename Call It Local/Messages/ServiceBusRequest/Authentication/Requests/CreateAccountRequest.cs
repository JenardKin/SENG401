using Messages.NServiceBus.Commands;

using System;


namespace Messages.ServiceBusRequest.Authentication.Requests
{
    [Serializable]
    public class CreateAccountRequest : AuthenticationServiceRequest
    {
        public CreateAccountRequest(CreateAccount createCommand)
            : base(AuthenticationRequest.CreateAccount)
        {
            this.createCommand = createCommand;
        }

        /// <summary>
        /// NServiceBus command containg information needed to create a new account
        /// </summary>
        public CreateAccount createCommand;
    }
}
