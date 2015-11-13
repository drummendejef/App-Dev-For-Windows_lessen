using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using Model;

namespace FlagoramaService
{
    public sealed class FlagoramaService : IBackgroundTask
    {
        BackgroundTaskDeferral _serviceDeferral;

        private VoiceCommandServiceConnection _voiceCommandServiceConnection;

        private readonly ResourceLoader _resourceLoader = new ResourceLoader();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            Country selectedcountry;

            // We need deferal
            // On a besoin de deferal
            _serviceDeferral = taskInstance.GetDeferral();

            // If cancelled, set deferal
            // Mets le déféral si annulation
            taskInstance.Canceled += (sender, reason) => _serviceDeferral?.Complete();

            // Get the details of the event that trigered the service
            // Obtient les détails de l'évenement qui à démarré le service
            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            // Check if it is service name set in VCD
            // Regarde si c'est le nom du service qui est mis dans le VCD
            if (triggerDetails?.Name == "FlagoramaService")
            {
                _voiceCommandServiceConnection =
                    VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                // Set deferal when voice command is completed
                // Mets le deferal quand la commande vocale est terminée
                _voiceCommandServiceConnection.VoiceCommandCompleted += (sender, args) => _serviceDeferral?.Complete();
                // Get voice command
                // Obtient la commande vocale
                var voicecommand = await _voiceCommandServiceConnection.GetVoiceCommandAsync();

                switch (voicecommand.CommandName)
                {
                    case "showFlagList":
                        // Show flag list
                        selectedcountry = await ShowFlagListAsync();
                        if (selectedcountry != null)
                        {
                            // If one flag is selected, ask confirmation
                            if (await PromptForConfirmationAsync(selectedcountry))
                            {
                                // Send success message with selected flag
                                await SendSuccessMessageAsync(selectedcountry);
                            }
                        }
                        break;
                    case "showFlag":
                        await SendErrorMessageAsync();
                        await Task.Delay(2000);
                        // Get country flags with selected name
                        var flags = Countries.List.Where(
                                d => d.Name.ToLower().Contains(voicecommand.Properties["flag"][0].ToLower())).ToList();
                        // If more than one, disambiguate
                        if (flags.Count() == 1)
                            selectedcountry = flags.First();
                        else
                        {
                            selectedcountry = await DisambiguateCountry(flags);
                        }
                        // Show progress message for 2 seconds, then show flag
                        await SendProgressMessageAsync(selectedcountry);
                        await Task.Delay(20000);
                        await SendSuccessMessageAsync(selectedcountry);
                        break;
                }
            }
        }

        private async Task<Country> DisambiguateCountry(List<Country> countries)
        {
            // The prompt message.
            // Le message à afficher
            var userPrompt = new VoiceCommandUserMessage();
            userPrompt.DisplayMessage =
                userPrompt.SpokenMessage = GetString(Strings.DisambiguateMessage);

            // Re-prompt message (In case the user sent an unknown command)
            // Message de répétition (au cas ou l'utilisateur a donné une commande inconnue)
            var userReprompt = new VoiceCommandUserMessage();
            userPrompt.DisplayMessage =
                userPrompt.SpokenMessage = GetString(Strings.DisambiguateRepeatMessage);

            var tilelist = new List<VoiceCommandContentTile>();

            // Get tile for each country flag
            // Genère une tuile pour chaque drapeaux
            foreach (var country in countries)
            {
                var tile = await GetTileAsync(country);

                tilelist.Add(tile);
            }

            // Create the response
            // Crée la réponse
            var response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, userReprompt, tilelist);

            // Ask the user to desambiguate
            // Demande à l'utilisateur de clarifier son choix
            var voiceCommandDisambiguationResult = await
                _voiceCommandServiceConnection.RequestDisambiguationAsync(response);

            // Return selected choice (or null)
            // Renvoie le choix selectionné (ou null)
            return (Country)voiceCommandDisambiguationResult?.SelectedItem.AppContext;
        }


        private async Task<Country> ShowFlagListAsync()
        {
            // Pompt and repeat promt
            // L'avis et la répétition de l'avis
            var promptmessage = new VoiceCommandUserMessage();
            promptmessage.DisplayMessage = promptmessage.SpokenMessage = GetString(Strings.FlagListMessage);

            var repeatpromptmessage = new VoiceCommandUserMessage();
            promptmessage.DisplayMessage = promptmessage.SpokenMessage = GetString(Strings.FlagListMessageRepeat);

            var tilelist = new List<VoiceCommandContentTile>();

            // Get tile for each country flag
            // Genère une tuile pour chaque drapeaux
            foreach (var country in Countries.List)
            {
                var tile = await GetTileAsync(country);

                tilelist.Add(tile);
            }

            // Create a prompt response message
            // Crée un message réponse prompt (c'est a dire qui pose une question)
            var response = VoiceCommandResponse.CreateResponseForPrompt(promptmessage, repeatpromptmessage, tilelist);

            // Show and get answer from user
            // L'affiche et attend la réponse de l'utilisateur
            var result = await _voiceCommandServiceConnection.RequestDisambiguationAsync(response);

            // Return the selected item (or null)
            // Renvoie l'élément choisi (ou null)
            return result?.SelectedItem.AppContext as Country;
        }

        private async Task<bool> PromptForConfirmationAsync(Country selectedCountry)
        {
            // Pompt and repeat promt
            // L'avis et la répétition de l'avis
            var confirmationmessage = new VoiceCommandUserMessage();
            confirmationmessage.DisplayMessage = confirmationmessage.SpokenMessage = string.Format(GetString(Strings.ConfirmationRepeatFlag), selectedCountry.Name);

            var repeatconfirmationmessage = new VoiceCommandUserMessage();
            repeatconfirmationmessage.DisplayMessage = repeatconfirmationmessage.SpokenMessage = "fffff";

            // Get flag tile
            // La tuile du drapeau
            var tile = await GetTileAsync(selectedCountry);

            // Ask for confirmation
            // Demande la confirmation
            var response = VoiceCommandResponse.CreateResponseForPrompt(confirmationmessage, repeatconfirmationmessage, new List<VoiceCommandContentTile> { tile });
            var confirmation = await _voiceCommandServiceConnection.RequestConfirmationAsync(response);

            return confirmation?.Confirmed ?? false;
        }

        private async Task SendSuccessMessageAsync(Country selectedCountry)
        {
            var confirmationmessage = new VoiceCommandUserMessage();

            // Get selected flag tile
            // Obtient la tuile du drapeau selectionné
            var tile = await GetTileAsync(selectedCountry);

            confirmationmessage.DisplayMessage = confirmationmessage.SpokenMessage = string.Format(GetString(Strings.SelectedFlag), selectedCountry.Name);

            // Show the success message
            // Affiche le message de succès
            var response = VoiceCommandResponse.CreateResponse(confirmationmessage, new List<VoiceCommandContentTile> { tile });
            response.AppLaunchArgument = "selection=none";
            await _voiceCommandServiceConnection.ReportSuccessAsync(response);
        }

        private async Task SendProgressMessageAsync(Country selectedCountry)
        {
            var progressmessage = new VoiceCommandUserMessage();

            progressmessage.DisplayMessage =
                progressmessage.SpokenMessage = string.Format(GetString(Strings.LoadingFlag), selectedCountry.Name);
            // Show progress message
            // Affiche le message de progression
            var response = VoiceCommandResponse.CreateResponse(progressmessage);

            await _voiceCommandServiceConnection.ReportProgressAsync(response);
        }

        private async Task SendErrorMessageAsync()
        {
            var progressmessage = new VoiceCommandUserMessage();

            progressmessage.DisplayMessage =
                progressmessage.SpokenMessage = GetString(Strings.ErrorMessage);
            // Show error message
            // Affiche le message d'erreur
            var response = VoiceCommandResponse.CreateResponse(progressmessage, new List<VoiceCommandContentTile> { await GetErrorTileAsync() });

            await _voiceCommandServiceConnection.ReportFailureAsync(response);
        }

        private async Task LaunchAsync(Country selectedCountry)
        {
            var userMessage = new VoiceCommandUserMessage();
            userMessage.DisplayMessage = userMessage.SpokenMessage = "Launching app"; ;

            var response = VoiceCommandResponse.CreateResponse(userMessage);

            response.AppLaunchArgument = selectedCountry.ID;
            // Launch app
            // Lance l'app
            await _voiceCommandServiceConnection.RequestAppLaunchAsync(response);
        }

        private async Task<VoiceCommandContentTile> GetTileAsync(Country country)
        {
            // Build a tile from selected country flag
            // Construit une tuile à partir du drapeau selectionné
            return new VoiceCommandContentTile
            {
                ContentTileType = VoiceCommandContentTileType.TitleWith68x68IconAndText,
                AppContext = country,
                AppLaunchArgument = "selectedid=" + country.ID,
                Title = string.Format(GetString(Strings.FlagOf), country.Name),
                TextLine1 = country.Details,
                Image = await StorageFile.GetFileFromApplicationUriAsync(
                    new Uri($"ms-appx:///FlagoramaService/Assets/{country.FlagIcon}"))
            };
        }

        private async Task<VoiceCommandContentTile> GetErrorTileAsync()
        {
            // Build an error tile
            // Construit une tuile erreur
            return new VoiceCommandContentTile
            {
                ContentTileType = VoiceCommandContentTileType.TitleWith280x140IconAndText,
                Title = GetString(Strings.ErrorMessage),
                TextLine1 = GetString(Strings.ErrorMessageDetails),
                Image = await StorageFile.GetFileFromApplicationUriAsync(
                    new Uri($"ms-appx:///FlagoramaService/Assets/Bug.png"))
            };
        }

        private string GetString(string key)
        {
            // Get string from resource
            // Obtient la chaine de caractère depuis la resource
            return _resourceLoader.GetString(key);
        }
    }
}
