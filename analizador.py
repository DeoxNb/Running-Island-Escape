import customtkinter as ctk
import pandas as pd
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
import threading
import firebase_admin
from firebase_admin import credentials, firestore
import os
import sys

ctk.set_appearance_mode("Dark")
ctk.set_default_color_theme("blue")

plt.style.use('dark_background')
plt.rcParams.update({
    'figure.facecolor': '#242424',
    'axes.facecolor': '#2b2b2b',
    'axes.edgecolor': '#404040',
    'text.color': '#E0E0E0',
    'xtick.color': '#A0A0A0',
    'ytick.color': '#A0A0A0',
    'font.family': 'sans-serif',
    'font.size': 10
})

class App(ctk.CTk):
    def __init__(self):
        super().__init__()
        self.title("Running Island - Cloud Telemetría en Vivo")
        self.geometry("1200x800")
        self.minsize(1000, 600)

        # --- MAQUETACIÓN ---
        self.sidebar = ctk.CTkFrame(self, width=250, corner_radius=0, fg_color="#1a1a1a")
        self.sidebar.pack(side="left", fill="y")

        titulo = ctk.CTkLabel(self.sidebar, text="RUNNING ISLAND", font=("Arial Black", 20, "bold"), text_color="#3498db")
        titulo.pack(pady=(30, 5), padx=20)
        
        subtitulo = ctk.CTkLabel(self.sidebar, text="Live Cloud Analytics", font=("Arial", 12))
        subtitulo.pack(pady=(0, 40), padx=20)

        self.lbl_total = ctk.CTkLabel(self.sidebar, text="-", font=("Arial Black", 40), text_color="#2ecc71")
        self.lbl_total.pack(pady=(10, 0))
        ctk.CTkLabel(self.sidebar, text="PARTIDAS TOTALES", font=("Arial", 11, "bold")).pack(pady=(0, 30))

        self.lbl_victorias = ctk.CTkLabel(self.sidebar, text="-", font=("Arial Black", 40), text_color="#f1c40f")
        self.lbl_victorias.pack(pady=(10, 0))
        ctk.CTkLabel(self.sidebar, text="VICTORIAS", font=("Arial", 11, "bold")).pack(pady=(0, 30))

        self.lbl_tiempo = ctk.CTkLabel(self.sidebar, text="-", font=("Arial Black", 40), text_color="#e74c3c")
        self.lbl_tiempo.pack(pady=(10, 0))
        ctk.CTkLabel(self.sidebar, text="TIEMPO MEDIO", font=("Arial", 11, "bold")).pack(pady=(0, 30))

        self.lbl_estado = ctk.CTkLabel(self.sidebar, text="Iniciando...", font=("Consolas", 12), text_color="#95a5a6")
        self.lbl_estado.pack(side="bottom", pady=20)

        self.plot_frame = ctk.CTkFrame(self, fg_color="#242424")
        self.plot_frame.pack(side="right", fill="both", expand=True, padx=20, pady=20)

        self.after(100, self.iniciar_descarga_cloud)

    def iniciar_descarga_cloud(self):
        self.lbl_estado.configure(text="Conectando a Firebase...", text_color="#f1c40f")
        threading.Thread(target=self.descargar_datos_firebase, daemon=True).start()

    def descargar_datos_firebase(self):
        try:
            # Lógica para encontrar el JSON tanto en script como en ejecutable
            if getattr(sys, 'frozen', False):
                base_path = sys._MEIPASS
            else:
                base_path = os.path.dirname(os.path.abspath(__file__))
            
            ruta_credenciales = os.path.join(base_path, "firebase_credenciales.json")
            
            if not firebase_admin._apps:
                cred = credentials.Certificate(ruta_credenciales)
                firebase_admin.initialize_app(cred)

            db = firestore.client()
            docs = db.collection('partidas').stream()
            datos = [doc.to_dict() for doc in docs]
            
            self.after(0, self.actualizar_graficos, datos)

        except Exception as e:
            self.after(0, self.mostrar_error, str(e))

    def actualizar_graficos(self, datos):
        if not datos:
            self.mostrar_error("No hay partidas registradas en la nube.")
            return

        try:
            df = pd.DataFrame(datos)
            total_partidas = len(df)
            victorias = len(df[df['desenlace'] == 'Victoria'])
            t_promedio = df['tiempoJugado'].mean()

            self.lbl_total.configure(text=str(total_partidas))
            self.lbl_victorias.configure(text=str(victorias))
            self.lbl_tiempo.configure(text=f"{t_promedio:.1f}s")
            self.lbl_estado.configure(text="Sincronizado en Vivo", text_color="#2ecc71")

            for widget in self.plot_frame.winfo_children(): 
                widget.destroy()
                
            fig = plt.figure(figsize=(12, 7))
            
            ax1 = fig.add_subplot(2, 2, 1)
            df.groupby('nombreJugador')['tiempoJugado'].mean().plot(kind='barh', ax=ax1, color='#3498db')
            ax1.set_title("Tiempo Medio por Jugador", pad=15, fontweight='bold')
            ax1.set_ylabel("")

            ax2 = fig.add_subplot(2, 2, 2)
            rutas = df['rutaEscape'].value_counts()
            ax2.pie(rutas, labels=rutas.index, autopct='%1.0f%%', startangle=90, colors=['#9b59b6', '#e67e22', '#1abc9c'], pctdistance=0.85)
            centre_circle = plt.Circle((0,0),0.70,fc='#2b2b2b')
            fig.gca().add_artist(centre_circle)
            ax2.set_title("Uso de Rutas de Escape", pad=15, fontweight='bold')

            ax3 = fig.add_subplot(2, 2, 3)
            df_victorias = df[df['desenlace'] == 'Victoria']
            if not df_victorias.empty:
                df_victorias['personaje'].value_counts().plot(kind='bar', ax=ax3, color='#e74c3c')
            ax3.set_title("Victorias por Personaje", pad=15, fontweight='bold')
            ax3.tick_params(axis='x', rotation=0)

            ax4 = fig.add_subplot(2, 2, 4)
            df['desenlace'].value_counts().plot(kind='bar', ax=ax4, color=['#2ecc71', '#e74c3c'])
            ax4.set_title("Balance General", pad=15, fontweight='bold')
            ax4.tick_params(axis='x', rotation=0)

            plt.tight_layout(pad=3.0)
            
            canvas = FigureCanvasTkAgg(fig, master=self.plot_frame)
            canvas.draw()
            canvas.get_tk_widget().pack(fill="both", expand=True)

        except Exception as e:
            self.mostrar_error(f"Error procesando datos:\n{str(e)}")

    def mostrar_error(self, mensaje):
        for widget in self.plot_frame.winfo_children(): 
            widget.destroy()
        lbl = ctk.CTkLabel(self.plot_frame, text=mensaje, font=("Arial", 16), text_color="#e74c3c")
        lbl.pack(expand=True)
        self.lbl_estado.configure(text="Error de Sincronización", text_color="#e74c3c")

if __name__ == "__main__":
    app = App()
    app.mainloop()