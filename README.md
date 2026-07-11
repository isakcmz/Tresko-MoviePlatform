# Tresko - Sosyal Film ve Dizi Platformu 🎬

Tresko, kullanıcıların popüler filmleri arayabildiği, keşfedebildiği, kişisel izleme listelerini oluşturabildiği ve izledikleri yapımlara yorum/puan bırakarak diğer kullanıcılarla paylaşabildikleri modern ve sosyal bir film platformudur. 

Proje, **ASP.NET Core 9.0 MVC** mimarisi kullanılarak geliştirilmiş olup, arka planda **TMDb (The Movie Database) API** desteği ile çalışmaktadır.

---

## ✨ Özellikler

*   **TMDb Entegrasyonu:** Gerçek zamanlı film verileri, popüler filmler tablosu, film detayları, oyuncu listesi ve gelişmiş film arama motoru.
*   **Sosyal Aktivite Akışı (Feed / Home Page):** Ana sayfada, sitenin diğer üyelerinin yaptığı tüm güncel hareketler (izleme durumları, puanlamalar ve film yorumları) dinamik bir zaman tüneli olarak listelenir.
*   **Detaylı Film Sayfaları:** Filme dair özel puan ortalamaları, diğer kullanıcıların yaptığı değerlendirmeler ve derecelendirmeler.
*   **İzleme Listesi (Watchlist):** Kullanıcıların daha sonra izlemek üzere kaydetmek istedikleri filmleri yönettiği öncelik sıralamalı liste.
*   **İzlenenler Listesi (Watched List):** Kullanıcının izlemiş olduğu filmleri arşivlediği, kaç kez izlendiğini ve izleme notlarını takip edebildiği alan.
*   **Kullanıcı Profilleri:** Kullanıcıların son aktivitelerini, izledikleri ve izlemek istedikleri popüler yapımları tek panelden yönettiği profil sayfası.
*   **ASP.NET Core Identity UI:** Şifreli ve güvenli üye kaydı, giriş/çıkış modülleri ve oturum yönetimi.
*   **SQLite Veri Tabanı:** Local geliştirme için hafif ve taşınabilir Entity Framework Code-First mimarisi.

---

## 🛠️ Kullanılan Teknolojiler

- **Backend Framework:** .NET 9.0 (ASP.NET Core MVC)
- **Database (Veri Tabanı) & ORM:** EF Core (Entity Framework Core) & SQLite
- **Security & Authorization:** ASP.NET Core Identity
- **API Client:** HttpClient (TMDb integration)
- **Frontend Technologies:** MVC Razor Views, HTML5, CSS3 (Custom Styles), JavaScript, Bootstrap 5

---

## ⚙️ Kurulum ve Çalıştırma

Projeyi yerel bilgisayarınızda çalıştırmak için aşağıdaki adımları sırasıyla uygulayabilirsiniz:

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/KULLANICI_ADINIZ/reponuz.git
cd Tresko
```

### 2. Yapılandırma Dosyasını (appsettings.json) Hazırlayın
`FilmSitesi.Web` klasörünün içindeki `appsettings.json` dosyasında bulunan TMDb API anahtarını tanımlamanız gerekir.

Dosya içeriği şu şekildedir:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=filmsitesi.db"
  },
  "Tmdb": {
    "ApiKey": "BURAYA_TMDB_API_ANAHTARINIZI_YAZIN"
  }
}
```
> 💡 **Not:** Eğer kendi API anahtarınız yoksa [TMDb web sitesinden](https://www.themoviedb.org/) ücretsiz bir geliştirici hesabı oluşturup API Key sekmesinden anahtar alabilirsiniz.

### 3. Veri Tabanı Migrations İşlemlerini Uygulayın
Veri tabanı şablonunun oluşturulması ve tabloların SQLite tabanına aktarılması için proje dizininde terminali açıp şu komutu çalıştırın:

```bash
dotnet ef database update --project FilmSitesi.Web
```

### 4. Projeyi Çalıştırın
Tüm işlemler tamamlandıktan sonra projeyi başlatmak için şu komutu kullanın:

```bash
dotnet run --project FilmSitesi.Web
```

Tarayıcınızdan `http://localhost:5000` (veya terminalde belirtilen local port) adresine giderek platformu kullanmaya başlayabilirsiniz!

---

## 📂 Proje Klasör Yapısı

*   `Controllers/`: MVC Denetleyici sınıfları (Home, Movies, Profile controllers).
*   `Data/`: `AppDbContext` tanımı ve EF Core veri tabanı konfigürasyonları.
*   `Models/`: Veri tabanı varlıkları (`AppUser`, `Movie`, `Review`, `Activity` vb.) ve View-Model yapıları.
*   `Views/`: Razor sayfası arayüz tasarımları ve kısmi görünümler (Partial Views).
*   `Services/`: TMDb API ve iç film servis mantıklarını yürüten arayüzler ve sınıflar.
*   `wwwroot/`: Statik CSS stilleri, resimler ve site JS dosyaları.

---

## 📄 Lisans
Bu proje MIT lisansı ile lisanslanmıştır. Dilediğiniz gibi kullanıp geliştirebilirsiniz.
