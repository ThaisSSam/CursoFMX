import SidebarComponent from "../components/Sidebar";
import {
  Trash2,
  AlertTriangle,
  CheckSquare,
  Play,
  Bell,
  Settings,
} from "lucide-react";

interface DashboardScreenProps {
  onLogout: () => Promise<void> | void;
}

export default function DashboardScreen({ onLogout }: DashboardScreenProps) {
  return (
    <div className="text-white bg-[#0f172a] min-h-screen flex flex-row font-sans selection:bg-blue-500/30">
      <SidebarComponent onLogout={onLogout} />

      <div className="flex flex-col w-full overflow-y-auto max-h-screen">
        <header className="flex justify-between items-center border-b border-slate-800 bg-[#0f172a] px-8 py-5 sticky top-0 z-10">
          <div>
            <h1 className="text-xl font-bold text-slate-100 font-['Inter']">
              Dashboard
            </h1>
          </div>

          <div className="flex items-center gap-4 text-xs text-slate-400">
            <span>Última atualização: agora mesmo</span>
            <button className="p-2 bg-amber-500/10 text-amber-500 rounded-xl hover:bg-amber-500/20 transition-all relative">
              <Bell size={16} />
              <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-amber-500 rounded-full"></span>
            </button>
            <button className="p-2 bg-slate-800 text-slate-300 rounded-xl hover:bg-slate-700 transition-all">
              <Settings size={16} />
            </button>
            <div className="w-8 h-8 bg-blue-600 rounded-xl flex items-center justify-center text-white font-bold text-xs shadow-lg shadow-blue-500/10">
              MA
            </div>
          </div>
        </header>

        <main className="flex-1 p-6 space-y-6 bg-[#090d16]">
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <div className="border border-slate-800 rounded-xl p-5 bg-[#131b2e] flex flex-col justify-between h-32 relative group hover:border-slate-700 transition-all">
              <div className="flex justify-between items-start">
                <span className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">
                  Tarefas em aberto
                </span>
                <Trash2 size={16} className="text-amber-600/60" />
              </div>
              <div>
                <h2 className="text-3xl font-bold text-slate-100"></h2>
                <p className="text-[11px] text-slate-500 mt-1">
                </p>
              </div>
            </div>

            <div className="border border-slate-800 rounded-xl p-5 bg-[#131b2e] flex flex-col justify-between h-32 relative hover:border-slate-700 transition-all">
              <div className="flex justify-between items-start">
                <span className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">
                  Atrasadas
                </span>
                <AlertTriangle size={16} className="text-slate-500" />
              </div>
              <div>
                <h2 className="text-3xl font-bold text-rose-500"></h2>
                <p className="text-[11px] text-slate-500 mt-1">

                </p>
              </div>
            </div>

            <div className="border border-slate-800 rounded-xl p-5 bg-[#131b2e] flex flex-col justify-between h-32 relative hover:border-slate-700 transition-all">
              <div className="flex justify-between items-start">
                <span className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">
                  Concluídas hoje
                </span>
                <CheckSquare size={16} className="text-slate-500" />
              </div>
              <div>
                <h2 className="text-3xl font-bold text-emerald-400"></h2>
                <p className="text-[11px] text-slate-500 mt-1">

                </p>
              </div>
            </div>

            <div className="border border-slate-800 rounded-xl p-5 bg-[#131b2e] flex flex-col justify-between h-32 relative hover:border-slate-700 transition-all">
              <div className="flex justify-between items-start">
                <span className="text-[10px] font-bold text-slate-400 tracking-wider uppercase">
                  Em andamento
                </span>
                <Play size={16} className="text-slate-500" />
              </div>
              <div>
                <h2 className="text-3xl font-bold text-amber-400"></h2>
                <p className="text-[11px] text-slate-500 mt-1">

                </p>
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-5 gap-4">
            <div className="lg:col-span-3 border border-slate-800 rounded-xl p-5 bg-[#131b2e] flex flex-col justify-between">
              <h3 className="text-xs font-semibold text-slate-400 uppercase tracking-wider mb-5">
                Tarefas por Responsável
              </h3>

              <div className="space-y-4 flex-1 flex flex-col justify-center">
                <div className="space-y-1.5">
                  <div className="flex justify-between text-xs">
                    <div className="flex items-center gap-2">
                      <div className="w-5 h-5 bg-amber-600/30 text-amber-500 text-[10px] font-bold rounded-full flex items-center justify-center">
                      </div>
                      <span className="text-slate-300 font-medium">
                      </span>
                    </div>
                    <span className="text-slate-500"></span>
                  </div>
                  <div className="w-full bg-slate-800/40 h-2 rounded-full overflow-hidden">
                    <div
                      className="bg-blue-500 h-full rounded-full"
                      style={{ width: "85%" }}
                    ></div>
                  </div>
                </div>

                <div className="space-y-1.5">
                  <div className="flex justify-between text-xs">
                    <div className="flex items-center gap-2">
                      <div className="w-5 h-5 bg-emerald-600/30 text-emerald-400 text-[10px] font-bold rounded-full flex items-center justify-center">
                      </div>
                      <span className="text-slate-300 font-medium">
                      </span>
                    </div>
                    <span className="text-slate-500"></span>
                  </div>
                  <div className="w-full bg-slate-800/40 h-2 rounded-full overflow-hidden">
                    <div
                      className="bg-purple-500 h-full rounded-full"
                      style={{ width: "72%" }}
                    ></div>
                  </div>
                </div>

                <div className="space-y-1.5">
                  <div className="flex justify-between text-xs">
                    <div className="flex items-center gap-2">
                      <div className="w-5 h-5 bg-blue-600/30 text-blue-400 text-[10px] font-bold rounded-full flex items-center justify-center">
                      
                      </div>
                      <span className="text-slate-300 font-medium">
                    
                      </span>
                    </div>
                    <span className="text-slate-500"></span>
                  </div>
                  <div className="w-full bg-slate-800/40 h-2 rounded-full overflow-hidden">
                    <div
                      className="bg-cyan-500 h-full rounded-full"
                      style={{ width: "60%" }}
                    ></div>
                  </div>
                </div>

                <div className="space-y-1.5">
                  <div className="flex justify-between text-xs">
                    <div className="flex items-center gap-2">
                      <div className="w-5 h-5 bg-rose-600/30 text-rose-400 text-[10px] font-bold rounded-full flex items-center justify-center">
                        
                      </div>
                      <span className="text-slate-300 font-medium">
                         
                      </span>
                    </div>
                    <span className="text-slate-500"> </span>
                  </div>
                  <div className="w-full bg-slate-800/40 h-2 rounded-full overflow-hidden">
                    <div
                      className="bg-emerald-400 h-full rounded-full"
                      style={{ width: "48%" }}
                    ></div>
                  </div>
                </div>

                <div className="space-y-1.5">
                  <div className="flex justify-between text-xs">
                    <div className="flex items-center gap-2">
                      <div className="w-5 h-5 bg-purple-600/30 text-purple-400 text-[10px] font-bold rounded-full flex items-center justify-center">
                        
                      </div>
                      <span className="text-slate-300 font-medium">
                         
                      </span>
                    </div>
                    <span className="text-slate-500"> </span>
                  </div>
                  <div className="w-full bg-slate-800/40 h-2 rounded-full overflow-hidden">
                    <div
                      className="bg-fuchsia-500 h-full rounded-full"
                      style={{ width: "38%" }}
                    ></div>
                  </div>
                </div>

                <div className="space-y-1.5">
                  <div className="flex justify-between text-xs">
                    <div className="flex items-center gap-2">
                      <div className="w-5 h-5 bg-cyan-600/30 text-cyan-400 text-[10px] font-bold rounded-full flex items-center justify-center">
                        
                      </div>
                      <span className="text-slate-300 font-medium">
                        
                      </span>
                    </div>
                    <span className="text-slate-500"> </span>
                  </div>
                  <div className="w-full bg-slate-800/40 h-2 rounded-full overflow-hidden">
                    <div
                      className="bg-orange-500 h-full rounded-full"
                      style={{ width: "22%" }}
                    ></div>
                  </div>
                </div>
              </div>
            </div>

            <div className="lg:col-span-2 border border-slate-800 rounded-xl p-5 bg-[#131b2e] flex flex-col justify-between">
              <h3 className="text-xs font-semibold text-slate-400 uppercase tracking-wider mb-4">
                Tarefas por Prioridade
              </h3>

              <div className="flex flex-col items-center justify-center flex-1 py-2">
                <div
                  className="w-36 h-36 rounded-full flex items-center justify-center relative shadow-inner"
                  style={{
                    background:
                      "conic-gradient(#ef4444 0% 18%, #f59e0b 18% 35%, #3b82f6 35% 62%, #10b981 62% 100%)",
                  }}
                >
                  <div className="w-24 h-24 bg-[#131b2e] rounded-full flex flex-col items-center justify-center shadow-lg">
                    <span className="text-2xl font-bold text-slate-100 leading-none">
                      
                    </span>
                    <span className="text-[9px] text-slate-500 uppercase tracking-wider mt-0.5">
                      total
                    </span>
                  </div>
                </div>
              </div>

              <div className="grid grid-cols-1 gap-1 text-[11px] text-slate-400 mt-4 border-t border-slate-800/60 pt-3">
                <div className="flex justify-between items-center">
                  <div className="flex items-center gap-1.5">
                    <span className="w-2 h-2 rounded-full bg-rose-500"></span>
                    <span>Crítica</span>
                  </div>
                  <span className="font-medium text-slate-300">
                     <span className="text-slate-600 ml-1"></span>
                  </span>
                </div>
                <div className="flex justify-between items-center">
                  <div className="flex items-center gap-1.5">
                    <span className="w-2 h-2 rounded-full bg-amber-500"></span>
                    <span>Alta</span>
                  </div>
                  <span className="font-medium text-slate-300">
                     <span className="text-slate-600 ml-1"></span>
                  </span>
                </div>
                <div className="flex justify-between items-center">
                  <div className="flex items-center gap-1.5">
                    <span className="w-2 h-2 rounded-full bg-blue-500"></span>
                    <span>Média</span>
                  </div>
                  <span className="font-medium text-slate-300">
                     <span className="text-slate-600 ml-1"></span>
                  </span>
                </div>
                <div className="flex justify-between items-center">
                  <div className="flex items-center gap-1.5">
                    <span className="w-2 h-2 rounded-full bg-emerald-500"></span>
                    <span>Baixa</span>
                  </div>
                  <span className="font-medium text-slate-300">
                     <span className="text-slate-600 ml-1"></span>
                  </span>
                </div>
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-4">
            <div className="w-full border border-slate-800 rounded-xl p-5 bg-[#131b2e]">
              <h3 className="text-xs font-semibold text-slate-400 uppercase tracking-wider mb-6">
                Evolução semanal de conclusões
              </h3>

              <div className="flex items-end justify-between h-28 px-4 w-full gap-2">
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "35%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "55%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "20%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    T5
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "70%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "45%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "65%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "30%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "80%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "50%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "88%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "95%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-slate-800/40 rounded-t hover:bg-slate-700/60 transition-all"
                    style={{ height: "40%" }}
                  ></div>
                  <span className="text-[9px] text-slate-600 font-medium">
                    
                  </span>
                </div>
                {/* A última barra em destaque com gradiente igual à imagem */}
                <div className="flex-1 flex flex-col items-center gap-2 h-full justify-end">
                  <div
                    className="w-full bg-gradient-to-t from-indigo-600 to-purple-400 rounded-t shadow-md shadow-indigo-500/10"
                    style={{ height: "60%" }}
                  ></div>
                  <span className="text-[9px] text-indigo-400 font-semibold">
                    
                  </span>
                </div>
              </div>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
}
