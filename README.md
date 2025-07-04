# GridMatchGame
## 📦 Proje Genel Bakış  
Bu proje bir **nxm** boyutunda grid sistemi üzerine kurulu, kullanıcı tıklamalarıyla “X” işaretleri koyup 3 veya daha fazla işaretli hücreyi temizleyen bir mini oyun altyapısıdır.  
- **Amaç:** Editör veya runtime’ta grid boyutunu ve havuz büyüklüğünü değiştirebilmek; kameraya her zaman sığan, performant ve kolay genişletilebilir bir sistem oluşturmak.  
- **Teknolojiler:** Unity (URP), Zenject (DI),  

---

## 🏗️ Mimari Katmanları  
1. **GridService**  
   -- Nesne havuzu (object pool) yönetimi  
   -- Hücre prefab’larının instantiate/clear işlemleri  
   -- `GenerateGrid()`, `ClearGrid()`, `PrewarmPool()` metodları  
2. **GridPresenter**  
   -- Kullanıcı tıklamalarını alır  
   -- Flood‐fill algoritmasıyla eşleşme kontrolü  
   -- Eşleşme temizleme ve skor servisine haber verme  
3. **NodeView**  
   -- Tek bir hücreyi temsil eder (SpriteRenderer + collider)  
   -- `SetMarked()` ile sprite değiştirme  
   -- Tıklamayı `OnMouseUpAsButton()` aracılığıyla Presenter’a iletme  
4. **ScoreService**  
   -- Toplam match sayısını tutar  
   -- UI (TextMeshProUGUI) üzerinden günceller  
5. **CameraGridFitter**  
   -- Grid boyutuna göre ortographic kamera boyutunu ve pozisyonunu ayarlar  
6. **GameInstaller (Zenject MonoInstaller)**  
   -- Sahnedeki servis ve bileşenleri Singleton olarak bind eder  

---

## 🎨 Kullanılan Tasarım Kalıpları  
-- **MVP (Model-View-Presenter)**  
   - _Model:_ GridService (veri ve havuz)  
   - _View:_ NodeView (UI & input)  
   - _Presenter:_ GridPresenter (iş mantığı, flood-fill)  
-- **Object Pool**  
   - Hücre prefab’ları runtime’da sürekli instantiate/destroy yerine yeniden kullanılır  
-- **Singleton Service**  
   - Zenject ile `GridService`, `ScoreService`, `CameraGridFitter`, `GridPresenter` single instance olarak enjekte edilir  

---

## 🔗 Dependency Injection (Zenject)  
-- **MonoInstaller** kullanarak sahnedeki komponentler `FromComponentInHierarchy().AsSingle()` ile bind edilir  
-- Bağımlılıklar `[Inject]` ile constructor veya metod da geçilir  

---

## 🛠️ Servisler ve Sorumluluklar  
### GridService  
-- Havuz ön-doldurma (`PrewarmPool`)  
-- Grid oluşturma/temizleme, board instantiate  
### GridPresenter  
-- Tıklama event’i alır, flood-fill ile komşuları bulur  
-- Eşleşmelerin sayısını hesaplayıp `ScoreService`’e bildirir  
### ScoreService  
-- Match sayısını tutar ve UI günceller  
### CameraGridFitter  
-- `LateUpdate` içinde ekran boyutu ve grid’e göre `orthographicSize` hesaplar  
-- Kamera pozisyonunu grid merkezine taşır  

---

## 🚀 Kullanım Akışı  
1. **Edit Mode**  
   -- Inspector’dan `width`, `height`, `prewarmCount` ayarları  
   -- Prewarm Pool → Build Grid → Clear Grid butonları  
   -- Sahneyi kaydedip Play Mode’a geçildiğinde aynı grid korunur  
2. **Play Mode**  
   -- Runtime’da Inspector’dan değerleri değiştirebilirsiniz  
   -- Butonlarla havuz ve grid yeniden oluşturulur  
   -- Tıklayınca “X” işareti, 3+ eşleşince temizleme ve skor artışı  

---
