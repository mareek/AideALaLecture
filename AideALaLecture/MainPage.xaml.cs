using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace AideALaLecture
{
    public sealed partial class MainPage : Page
    {
        private string[] _tousLesMots =
        {
            "caramel", "cartable", "mardi", "la", "chat", "chaud", "lavabo", "canard", "plume", "table", "pipe", "vélo","écureuil", "carotte",
            "chocolat", "maman", "garçon", "un", "parachute", "baignoire", "caméra", "cafetière", "cerises", "des", "les", "le", "maîtresse",
            "mercredi", "rouge", "vendredi", "sur", "vert", "lire", "mari", "car", "midi", "fourmi", "chemise", "navire", "cheminée", "ami",
            "miche", "livre", "salive", "olive", "maline", "sali", "limace", "tigre", "souris, petit", "sourire", "farine", "niche", "aride",
            "pari", "rideau", "bobine", "champignon", "confiture", "une", "jeudi", "samedi", "dimanche", "lit", "fille", "il y a", "je suis",
            "dix", "six", "bonhomme; pomme", "école", "dégringole", "s'envole", "dominos", "tomate", "mur", "camion", "moto", "momie", "melon",
            "marmite", "fantôme", "lapin", "locomotive", "pile", "télé", "luge", "lune", "lézard", "poule", "moule", "lame", "casserole",
            "je m'appelle", "policier", "guépard", "parapluie", "lampe", "quatre", "trois", "pourquoi", "pirate", "poisson", "papa", "tulipe",
            "septembre", "tapis", "loup", "sept", "genou", "fourchette", "chouette", "coupe", "semoule", "autoroute", "écouter", "ours", "rouler",
            "route", "poussin", "pour", "bonjour", "mouche", "toucher", "bouche", "boule", "cour", "tout", "jouer", "soupe", "rousse", "enfant",
            "igloo", "football", "cheval", "chapeau", "chèvre", "château", "chou", "vache", "chameau", "chaton", "cochon", "chut", "cacher",
            "short", "shampoing", "histoire", "hiver", "gros", "dos", "peau", "bureau", "radeau", "robot", "pinceau", "barreau", "mot ", "beau",
            "gros", "veau", "seau", "fauve", "sauter", "porter", "sortir", "poser", "voter", "dorer", "donner", "forêt", "rose", "zéro", "gauche",
            "jaune", "l'eau", "oiseau", "des cadeaux", "gâteau", "chapeau", "sortie", "bol", "bottes", "banane", "écrire", "nez", "wagon", "monstre",
            "pompier", "ballon", "navet", "cadenas", "dinosaure", "numéro", "animal", "renard", "piscine", "je nage", "la nuit", "noir", "automne",
            "grand", "cinq", "mare", "fée", "mère", "père", "chanter", "vous nagez", "dictée", "vous avez", "dîner", "fête", "janvier", "cuillère",
            "l'escalier", "saladier", "panier", "voyager", "clocher", "danser", "fusée", "poupée", "écolier", "plonger", "cerisier", "éléphant",
            "étoile", "bébé", "boulanger", "cahier", "mes", "tes", "ses", "pied", "avion", "caverne", "cravate", "soulever", "savonner", "narine",
            "aviron", "kiwi", "voleur", "vacances", "avocat", "village", "valise", "verre", "vingt", "wagon", "février", "champ", "moins", "train",
            "onze", "trompette", "gomme", "ronde : montre", "compote", "bombe", "gronde", "compte", "mouton", "bidon", "bonbon", "maison", "bouton",
            "crayons", "nombre", "avec", "parasol", "balai", "pédale", "bureau", "robinet", "pelle", "robe", "bougie", "cube", "biche", "hibou",
            "biberon", "radio", "bleu", "blanc", "bateau", "belle", "arbre", "tombe; novembre", "décembre", "balade", "zèbre", "serpent", "addition",
            "deuxième", "tortue", "joie", "jour", "joue", "juin", "jongle", "toupie", "mousse", "framboise", "citron", "ardoise", "boîte", "voilier",
            "voiture", "roi", "royaume", "moi", "toi", "le toit", "il était une fois", "joyeux", "mes doigts", "il doit", "elle voit", "froid",
            "mouchoir", "voici", "voilà", "devoirs", "poire", "point", "elle", "dans", "que", "c'est", "est", "deux", "était", "on", "ils", "ces",
            "ville", "qui", "des", "pain"
        };

        private const int MaxWordLength = 5;
        private static readonly TimeSpan TempsMax = TimeSpan.FromMinutes(5);

        private static readonly Dictionary<RapiditeRéponse, TimeSpan> TempsRéponses = new Dictionary<RapiditeRéponse, TimeSpan>
        {
            { RapiditeRéponse.Excellent,      TimeSpan.FromSeconds(5) },
            { RapiditeRéponse.Bon,            TimeSpan.FromSeconds(10) },
            { RapiditeRéponse.Correct,        TimeSpan.FromSeconds(30) },
            { RapiditeRéponse.Mauvais,        TimeSpan.FromMinutes(1) },
            { RapiditeRéponse.Catastrophique, TimeSpan.FromHours(24) },
        };

        private static readonly Dictionary<RapiditeRéponse, Uri> ImagesRéponses = new Dictionary<RapiditeRéponse, Uri>
        {
            { RapiditeRéponse.Excellent,      new Uri("ms-appx:/Assets/IronMan.jpg", UriKind.Absolute)},
            { RapiditeRéponse.Bon,            new Uri("ms-appx:/Assets/Superman.jpg", UriKind.Absolute) },
            { RapiditeRéponse.Correct,        new Uri("ms-appx:/Assets/TanteMay.jpg", UriKind.Absolute) },
            { RapiditeRéponse.Mauvais,        new Uri("ms-appx:/Assets/jjj.jpg", UriKind.Absolute) },
            { RapiditeRéponse.Catastrophique, new Uri("ms-appx:/Assets/LordBusiness.jpg", UriKind.Absolute) },
        };

        private readonly Stopwatch _chronoTotal = new Stopwatch();
        private readonly Stopwatch _chronoMot = new Stopwatch();
        private Random _rnd = new Random();
        private List<RapiditeRéponse> _resultats = new List<RapiditeRéponse>();
        private string _motCourant;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ShowAllImages();

            _chronoTotal.Start();
            ChangeMot();
        }

        private async Task ShowAllImages()
        {
            foreach (RapiditeRéponse reponse in Enum.GetValues(typeof(RapiditeRéponse)))
            {
                await Affiche(reponse, true);
            }
        }

        private void ChangeMot()
        {
            var motPrécédent = _motCourant;
            do
            {
                _motCourant = _tousLesMots[_rnd.Next(_tousLesMots.Length)];
            }
            while (_motCourant == motPrécédent || _motCourant.Length > MaxWordLength);

            MotLabel.Text = _motCourant;
            _chronoMot.Restart();
        }

        private RapiditeRéponse MesureTemps()
        {
            _chronoMot.Stop();
            var tempsReponse = _chronoMot.Elapsed;
            return GetRapiditeRéponse(tempsReponse);
        }

        private RapiditeRéponse GetRapiditeRéponse(TimeSpan tempsReponse)
        {
            return TempsRéponses.OrderBy(t => t.Value).SkipWhile(t => t.Value < tempsReponse).First().Key;
        }

        private async Task Affiche(RapiditeRéponse resultat, bool subliminal = false)
        {
            Uri imagePath;
            if (ImagesRéponses.TryGetValue(resultat, out imagePath))
            {
                ImageSubliminale.Source = new BitmapImage(imagePath);
                ImageSubliminale.Visibility = Visibility.Visible;
                if (subliminal)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(300));
                    ImageSubliminale.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async Task AfficheScore()
        {
            var scoreFinal = GetRapiditeRéponse(new TimeSpan(TempsMax.Ticks / _resultats.Count));
            await Affiche(scoreFinal);
        }

        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var resultat = MesureTemps();
            if (_chronoTotal.Elapsed > TempsMax)
            {
                await AfficheScore();
            }
            else
            {
                _resultats.Add(resultat);
                await Affiche(resultat, true);
                ChangeMot();
            }
        }

        private enum RapiditeRéponse
        {
            Excellent,
            Bon,
            Correct,
            Mauvais,
            Catastrophique
        }
    }
}
