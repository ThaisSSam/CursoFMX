import { useNavigate } from "react-router-dom";
import SidebarComponent from "../../../components/Sidebar";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Settings, Bell, ArrowLeft } from "lucide-react";
import TarefaForm from "./tarefaForm"; 

interface CadastroTarefaScreenProps {
  onLogout: () => Promise<void> | void;
}

export default function CadastroTarefaScreen({ onLogout }: CadastroTarefaScreenProps) {
  const navigate = useNavigate();

  function handleSucesso() { 
    navigate("/tarefas", { 
      state: { 
        mensagemSucesso: "Tarefa cadastrada com sucesso!" 
      } 
    });
  }

  return (
    <div className="text-white bg-[#0f172a] min-h-screen flex flex-row font-sans selection:bg-blue-500/30">
      <SidebarComponent
        currentPath="/tarefas"
        onNavigate={(path) => navigate(path)}
        onLogout={onLogout}
      />      

      <div className="flex flex-col w-full overflow-y-auto max-h-screen">
        <header className="flex justify-between items-center border-b border-slate-800 bg-[#0f172a] px-8 py-5 sticky top-0 z-10">
          <div className="flex items-center gap-3">
            <Button
              variant="ghost"
              size="icon"
              onClick={() => navigate("/tarefas")}
              className="text-slate-400 hover:text-white hover:bg-slate-800 rounded-xl"
            >
              <ArrowLeft size={18} />
            </Button>
            <h1 className="text-xl font-bold text-slate-100 font-['Inter']">
              Nova Tarefa
            </h1>
          </div>

          <div className="flex items-center gap-4 text-xs text-slate-400">
            <Button
              variant="ghost"
              size="icon"
              className="bg-amber-500/10 text-amber-500 hover:bg-amber-500/20 rounded-xl relative"
            >
              <Bell size={16} />
              <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-amber-500 rounded-full"></span>
            </Button>

            <Button
              variant="ghost"
              size="icon"
              className="bg-slate-800 text-slate-300 hover:bg-slate-700 rounded-xl"
            >
              <Settings size={16} />
            </Button>

            <Avatar className="w-8 h-8 rounded-xl shadow-lg shadow-blue-500/10">
              <AvatarFallback className="bg-blue-600 text-white font-bold text-xs rounded-xl">
                TH
              </AvatarFallback>
            </Avatar>
          </div>
        </header>

        <main className="flex-1 p-6 bg-[#090d16] flex items-center justify-center">
          <Card className="w-full max-w-xl border-slate-800 bg-[#131b2e] text-white p-5 shadow-xl shadow-black/20 animate-in fade-in-50 duration-200">
            <CardHeader className="p-0 mb-6">
              <CardTitle className="text-sm font-semibold text-slate-400 uppercase tracking-wider">
                Preencha os dados da atividade
              </CardTitle>
            </CardHeader>
            <CardContent className="p-0">
              
              <TarefaForm 
                onSucesso={handleSucesso} 
                onCancelar={() => navigate("/tarefas")} 
              />

            </CardContent>
          </Card>
        </main>
      </div>
    </div>
  );
}