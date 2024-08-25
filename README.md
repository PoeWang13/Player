# Player
Player sistemidir. Player için hareket, ateş, bıçaklama ve bomba atma sistemleri vardır.

# Ateş sistemleri
F ile düşamana belli mesafeden uzaksak baktığımız yöne ateş edilir ama yakınsak bıçak çekilir. 0.5 saniyede bir ateş edilir, bu zamanı Inpectorden değiştirebiliriz. Bıçak çekim ve kurşun atım noktalarını Controllerdan değiştirebiliriz.
B ile bomba atılır. 0.5 saniyede bir atılır, bu zamanı Inpectorden değiştirebiliriz. Bomba atım noktasını Controllerdan değiştirebiliriz.

# Hareket sistemleri
Offset : Karakterin BoxCollider'ın offset'dir.
Sizse : Karakterin BoxCollider'ın size'dır. 
Statelerde, state'in durumuna göre sınırlar ve kenar kaymalar değişebilir bu sebeple BoxCollider komponentini her state değişiminde state'e göre değiştirmeliyiz.
Dash ve Slide süresi hızı Inpectorden değişebilir.
# Walking
Ayakta yürüme sistemidir. C tuşu ile hızlı yürüme yapılabilinir. V ile Dash-Kayma yapılabilinir. Yürüme hızı ve yürüme açı limiti değişebilir.

# Sitting
Otururken yürüme sistemidir. V ile Slide-Kayma yapılabilinir. Yürüme hızı ve yürüme açı limiti değişebilir.

# Jumping
Zıplama sistemidir. V ile Dash yapılabilinir. Max zıplama sayısı, zıplama gücü değişebilir.

# Falling
Düşme sistemidir. V ile Dash yapılabilinir.

# Gravity
Değişik açılardaki yerçekimi sistemidir. Sistem şu anda aktif değildir çünkü 20 derecelik eğimde düzgün çalışmasına rağmen 40 derecelik eğime geçerken sistem sapıtmaktadır.

# Roping Right
İpte sağdan bakış açısı ile ilerleme sistemidir. 

# Roping Up
İpte yukarı doğru çıkarken sırttan bakış açısı ile ilerleme sistemidir. V ile Dash yapılabilinir.

# Spacing Right
Uzayda sağa doğru ilerlerken, sağdan bakış açısı ile ilerleme sistemidir. V ile Dash yapılabilinir.

# Spacing Up
Uzayda ileri doğru çıkarken sırttan bakış açısı ile ilerleme sistemidir. V ile Dash yapılabilinir.

# Walling Right
Duvarda yukarı doğru tırmanırken sağdan bakış açısı ile ilerleme sistemidir. V ile Dash yapılabilinir.

# Walling Up
Duvarda veya dağda yukarı doğru çıkarken sırttan bakış açısı ile ilerleme sistemidir. V ile Dash yapılabilinir.
