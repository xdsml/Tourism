using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using menu.Services;
using Microsoft.Maui.Controls.Shapes;
using Firebase.Database;
using Firebase.Database;
using Firebase.Database.Query;

namespace menu
{
    public partial class ChoixPhotoPage : ContentPage
    {
        private string? selectedFilePath;
        private CloudinaryService _cloudinaryService;
        private FirebasePostService _firebaseService = new FirebasePostService();
        private readonly FirebaseClient _firebaseClient = new FirebaseClient("https://guidsign-c2579-default-rtdb.europe-west1.firebasedatabase.app/");


        public ChoixPhotoPage()
        {
            InitializeComponent();
            _cloudinaryService = new CloudinaryService();
            _ = LoadMediaPosts();
        }

        private async void OnChooseFileClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Choose a picture",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    selectedFilePath = result.FullPath;
                    SelectedImage.Source = ImageSource.FromFile(selectedFilePath);
                    SelectedImage.IsVisible = true;
                    CaptionEntry.IsVisible = true;
                    PublishButton.IsVisible = true;
                    CancelButton.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        private async void OnPublishClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                await DisplayAlert("Erreur", "Veuillez choisir une image.", "OK");
                return;
            }

            try
            {
                var uploadedUrl = await _cloudinaryService.UploadFileAsync(selectedFilePath);

                string nom = Preferences.Get("nom", "");
                string prenom = Preferences.Get("prenom", "");
                string uid = Preferences.Get("uid", "");
                if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom))
                {
                    var userData = await _firebaseClient
                        .Child("users")
                        .Child(uid)
                        .OnceSingleAsync<UserModel>();

                    nom = userData?.Nom ?? "Inconnu";
                    prenom = userData?.Prenom ?? "Inconnu";

                    Preferences.Set("nom", nom);
                    Preferences.Set("prenom", prenom);
                }

                Console.WriteLine($"👤 Publie en tant que : {nom} {prenom}");

                await _firebaseService.AddPostAsync(new FirebasePost
                {
                    PostOwner = $"{nom} {prenom}",
                    userId = uid,
                    imageUrl = uploadedUrl,
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    likes = 0,
                    reports = 0,
                    commentsCount = 0,
                    caption = CaptionEntry.Text
                });

                await DisplayAlert("Succès", "Image publiée avec succès.", "OK");

                selectedFilePath = null;
                SelectedImage.IsVisible = false;
                CaptionEntry.IsVisible = false;
                PublishButton.IsVisible = false;
                CancelButton.IsVisible = false;

                await LoadMediaPosts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", ex.Message, "OK");
            }
        }
        


        private void OnCancelClicked(object sender, EventArgs e)
        {
            selectedFilePath = null;
            SelectedImage.IsVisible = false;
            CaptionEntry.IsVisible = false;
            PublishButton.IsVisible = false;
            CancelButton.IsVisible = false;
        }

        private async Task LoadMediaPosts()
        {
            var posts = await _firebaseService.GetAllPostsAsync();
            PostContainer.Children.Clear();

            foreach (var post in posts.Values)
            {
                if (!string.IsNullOrEmpty(post.imageUrl))
                {
                    var ownerRow = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        VerticalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new Image
                            {
                                Source = "avatar_placeholder.png",
                                WidthRequest = 40,
                                HeightRequest = 40,
                                Aspect = Aspect.AspectFill,
                                Clip = new EllipseGeometry
                                {
                                    Center = new Microsoft.Maui.Graphics.Point(20, 20),
                                    RadiusX = 20,
                                    RadiusY = 20
                                }
                            },
                            new Label
                            {
                                Text = post.PostOwner ?? "Utilisateur inconnu",
                                FontSize = 14,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Colors.DarkSlateGray,
                                VerticalOptions = LayoutOptions.Center
                            }
                        }
                    };

                    var image = new Image
                    {
                        Source = ImageSource.FromUri(new Uri(post.imageUrl)),
                        Aspect = Aspect.AspectFill,
                        HeightRequest = 250
                    };

                    var captionLabel = new Label
                    {
                        Text = post.caption ?? "",
                        FontSize = 14,
                        TextColor = Colors.Black,
                        Margin = new Thickness(10, 5, 10, 0)
                    };

                    var likeCountLabel = new Label { Text = post.likes.ToString(), FontSize = 12 };
                    var commentCountLabel = new Label { Text = post.commentsCount.ToString(), FontSize = 12 };

                    var likeStack = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 4,
                        Children =
                        {
                            new Button
                            {
                                ImageSource = post.IsLiked ? "favorite.png" : "heart.png",
                                BackgroundColor = Colors.Transparent,
                                WidthRequest = 40,
                                HeightRequest = 40,
                                Command = new Command(async () => await OnLikeClicked(post))
                            },
                            likeCountLabel
                        }
                    };

                    var commentStack = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 4,
                        Children =
                        {
                            new Button
                            {
                                ImageSource = "chat.png",
                                BackgroundColor = Colors.Transparent,
                                WidthRequest = 40,
                                HeightRequest = 40,
                                Command = new Command(async () => await OnCommentClicked(post))
                            },
                            commentCountLabel
                        }
                    };

                    var reportButton = new Button
                    {
                        ImageSource = "report.png",
                        BackgroundColor = Colors.Transparent,
                        WidthRequest = 40,
                        HeightRequest = 40,
                        Command = new Command(async () => await OnReportClicked(post))
                    };

                    var buttonLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 15,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = { likeStack, commentStack, reportButton }
                    };

                    var frame = new Frame
                    {
                        BorderColor = Colors.LightGray,
                        CornerRadius = 15,
                        Padding = 10,
                        Content = new StackLayout
                        {
                            Spacing = 20,
                            Children = { ownerRow, image, captionLabel, buttonLayout }
                        }
                    };

                    PostContainer.Children.Add(frame);
                }
            }
        }

        private async Task OnLikeClicked(FirebasePost post)
        {
            try
            {
                string userId = Preferences.Get("uid", "");
                await _firebaseService.ToggleLikeAsync(post.Id, userId);
                await LoadMediaPosts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Erreur Like/Unlike : {ex.Message}", "OK");
            }
        }

        private async Task OnCommentClicked(FirebasePost post)
        {
            var firebasePostService = new FirebasePostService();

            // Créer un layout principal modifiable
            var mainLayout = new StackLayout();

            // Fenêtre des commentaires
            var commentsWindow = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Colors.White,
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 300,
                HeightRequest = 400,
                Spacing = 10
            };

            // Grid pour titre + bouton de fermeture
            var headerGrid = new Grid
            {
                ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Auto }
        }
            };

            var titleLabel = new Label
            {
                Text = "Commentaires",
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            var closeButton = new ImageButton
            {
                Source = "close.png",
                BackgroundColor = Colors.Transparent,
                HeightRequest = 24,
                WidthRequest = 24,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };

            closeButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            };

            headerGrid.Add(titleLabel, 0, 0);
            headerGrid.Add(closeButton, 1, 0);

            commentsWindow.Children.Add(headerGrid);

            // Récupérer les commentaires
            var comments = await firebasePostService.GetCommentsAsync(post.Id);

            foreach (var comment in comments.Values)
            {
                var commentLabel = new Label
                {
                    Text = $"{comment.userId}: {comment.content}",
                    FontSize = 14,
                    TextColor = Colors.Black,
                    Padding = new Thickness(5),
                    VerticalOptions = LayoutOptions.Start
                };
                commentsWindow.Children.Add(commentLabel);
            }

            var commentEntry = new Entry
            {
                Placeholder = "Écrivez un commentaire...",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            commentsWindow.Children.Add(commentEntry);

            var sendButton = new Button
            {
                Text = "Envoyer",
                HorizontalOptions = LayoutOptions.End
            };

            sendButton.Clicked += async (sender, e) =>
            {
                var newComment = commentEntry.Text?.Trim();
                if (!string.IsNullOrEmpty(newComment))
                {
                    string uid = Preferences.Get("uid", "");
                    string nom = Preferences.Get("nom", "");
                    string prenom = Preferences.Get("prenom", "");

                    if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom))
                    {
                        var userData = await _firebaseClient
                            .Child($"users/{uid}")
                            .OnceSingleAsync<UserModel>();

                        nom = userData?.Nom ?? "Inconnu";
                        prenom = userData?.Prenom ?? "Inconnu";

                        Preferences.Set("nom", nom);
                        Preferences.Set("prenom", prenom);
                    }
                    var comment = new FirebaseComment
                    {
                        userId = $"{nom} {prenom}",
                        content = newComment,
                        timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()

                    };

                    await firebasePostService.AddCommentAsync(post.Id, comment);
                    await DisplayAlert("Succès", "Commentaire ajouté avec succès!", "OK");
                    commentEntry.Text = string.Empty;

                    var newCommentLabel = new Label
                    {
                        Text = $"Vous: {newComment}",
                        FontSize = 14,
                        TextColor = Colors.Black,
                        Padding = new Thickness(5),
                        VerticalOptions = LayoutOptions.Start
                    };
                    commentsWindow.Children.Insert(commentsWindow.Children.Count - 2, newCommentLabel);
                }
                else
                {
                    await DisplayAlert("Erreur", "Le commentaire ne peut pas être vide.", "OK");
                }
            };

            commentsWindow.Children.Add(sendButton);

            // Affichage de la fenêtre
            Content = new StackLayout
            {
                Children = { commentsWindow }
            };
        
        }

        private async Task OnReportClicked(FirebasePost post)
        {
            try
            {
                // 1. Préparer les données
                var reportReasons = new Dictionary<string, bool>
        {
            {"Contenu inapproprié", false},
            {"Spam ou tromperie", false},
            {"Discours haineux", false},
            {"Violence ou contenu dangereux", false},
            {"Autre", false}
        };

                // 2. Créer l'interface
                var content = new StackLayout { Spacing = 15, Padding = 20 };
                Entry otherReasonEntry = null;

                foreach (var reason in reportReasons.Keys)
                {
                    var checkbox = new CheckBox
                    {
                        Color = Colors.Black,
                        VerticalOptions = LayoutOptions.Center
                    };

                    var label = new Label
                    {
                        Text = reason,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = 16
                    };

                    var row = new HorizontalStackLayout
                    {
                        Spacing = 15,
                        Children = { checkbox, label }
                    };

                    if (reason == "Autre")
                    {
                        otherReasonEntry = new Entry
                        {
                            Placeholder = "Précisez la raison...",
                            IsVisible = false,
                            Margin = new Thickness(30, 0, 0, 0)
                        };

                        checkbox.CheckedChanged += (s, e) =>
                        {
                            reportReasons[reason] = e.Value;
                            otherReasonEntry.IsVisible = e.Value;
                        };

                        content.Children.Add(row);
                        content.Children.Add(otherReasonEntry);
                    }
                    else
                    {
                        checkbox.CheckedChanged += (s, e) => reportReasons[reason] = e.Value;
                        content.Children.Add(row);
                    }
                }

                // 3. Créer les boutons Valider/Annuler
                var validateButton = new Button
                {
                    Text = "Valider",
                    BackgroundColor = Colors.Black,
                    TextColor = Colors.White
                };

                var cancelButton = new Button
                {
                    Text = "Annuler",
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black
                };

                var buttonsLayout = new HorizontalStackLayout
                {
                    Spacing = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { validateButton, cancelButton }
                };

                content.Children.Add(buttonsLayout);

                // 4. Créer la fenêtre modale
                var popup = new ContentPage
                {
                    Content = new ScrollView
                    {
                        Content = new StackLayout
                        {
                            Children =
                    {
                        new Label
                        {
                            Text = "Pourquoi signalez-vous ce contenu ?",
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 18,
                            Margin = new Thickness(0, 0, 0, 15)
                        },
                        content
                    },
                            Padding = 20
                        }
                    }
                };

                // 5. Gérer la réponse
                bool result = false;
                var tcs = new TaskCompletionSource<bool>();

                validateButton.Clicked += (s, e) =>
                {
                    result = true;
                    tcs.SetResult(true);
                    Application.Current.MainPage.Navigation.PopModalAsync();
                };

                cancelButton.Clicked += (s, e) =>
                {
                    result = false;
                    tcs.SetResult(false);
                    Application.Current.MainPage.Navigation.PopModalAsync();
                };

                await Application.Current.MainPage.Navigation.PushModalAsync(popup);
                await tcs.Task;

                // 6. Traitement après fermeture
                if (result)
                {
                    var selectedReasons = reportReasons
                        .Where(r => r.Value)
                        .Select(r => r.Key == "Autre" ? $"Autre: {otherReasonEntry?.Text}" : r.Key)
                        .ToList();

                    if (selectedReasons.Count > 0)
                    {
                        await _firebaseService.AddReportAsync(
                            post.Id,
                            "d2NDIAFUqITzBkIZ1ttJyv9fdzp2",
                            selectedReasons);

                        await DisplayAlert("Merci", "Votre signalement a été pris en compte.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Attention", "Veuillez sélectionner au moins une raison.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", ex.Message, "OK");
            }
        }
    }
}
