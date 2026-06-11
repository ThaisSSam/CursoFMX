import { type ColumnDef } from '@tanstack/react-table';
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import {  Plus, Eye } from 'lucide-react';
import type { Tarefa } from "@/services/endpoints/tarefas";

export const DEFAULT_TAREFA_COLUMNS = [
  { id: 'select', label: 'Seleção', visible: true, fixed: true },
  { id: 'acoes', label: 'Ações', visible: true, fixed: true },
  { id: 'codigo', label: 'Código', visible: true },
  { id: 'nome', label: 'Título', visible: true },
  { id: 'responsavel', label: 'Responsável', visible: true },
  { id: 'prioridade', label: 'Prioridade', visible: true },
  { id: 'situacao', label: 'Situação', visible: true },
  { id: 'dataCriacao', label: 'Data Criação', visible: true },
];

const getPrioridadeConfig = (prio: number) => {
  switch (prio) {
    case 3: return { label: "Crítica", classes: "bg-red-500/10 text-red-500 border-red-500/20" };
    case 2: return { label: "Alta", classes: "bg-orange-500/10 text-orange-500 border-orange-500/20" };
    case 1: return { label: "Média", classes: "bg-amber-500/10 text-amber-500 border-amber-500/20" };
    default: return { label: "Baixa", classes: "bg-slate-500/10 text-slate-400 border-slate-500/20" };
  }
};

const getSituacaoConfig = (sit: number) => {
  switch (sit) {
    case 1: return { label: "A fazer", classes: "bg-slate-700 text-slate-300 border-slate-600" };
    case 2: return { label: "Em andamento", classes: "bg-blue-500/10 text-blue-400 border-blue-500/20" };
    case 3: return { label: "Concluída", classes: "bg-emerald-500/10 text-emerald-400 border-emerald-500/20" };
    case 4: return { label: "Bloqueada", classes: "bg-rose-500/10 text-rose-500 border-rose-500/20" };
    case 5: return { label: "Em validação", classes: "bg-purple-500/10 text-purple-400 border-purple-500/20" };
    default: return { label: "Backlog", classes: "bg-slate-800 text-slate-400 border-slate-700" };
  }
};

export const createTarefaColumns = (
  onViewClick?: (data: Tarefa) => void,
  onEditClick?: (data: Tarefa) => void,
): ColumnDef<Tarefa>[] => [
  {
    id: 'select',
    header: ({ table }) => (
      <Checkbox 
        checked={table.getIsAllRowsSelected()} 
        onCheckedChange={(value) => table.toggleAllRowsSelected(!!value)}
        className="border-slate-700 data-[state=checked]:bg-blue-600"
      />
    ),
    cell: ({ row }) => (
      <Checkbox 
        checked={row.getIsSelected()} 
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        className="border-slate-700 data-[state=checked]:bg-blue-600"
      />
    ),
    size: 50,
    enableSorting: false,
    enableResizing: false,
  },
  {
    id: 'acoes',
    header: 'Ações',
    cell: ({ row }) => (
      <div className="flex gap-1 justify-center">
        <Button 
          variant="ghost" 
          size="icon" 
          className="h-7 w-7 text-slate-500 hover:text-slate-300"
          onClick={() => onEditClick?.(row.original)}
        >
          <Plus size={14} />
        </Button>
        <Button 
          variant="ghost" 
          size="icon" 
          className="h-7 w-7 text-slate-500 hover:text-slate-300"
          onClick={() => onViewClick?.(row.original)}
        >
          <Eye size={14} />
        </Button>
      </div>
    ),
    size: 80,
    enableResizing: false,
  },
  {
    id: 'codigo',
    accessorKey: 'codigo',
    header: 'Código',
    cell: ({ getValue }) => <div className="font-mono text-slate-500 font-medium block truncate">TASK-{String(getValue()).padStart(4, '0')}</div>,
    size: 100,
    minSize: 90
  },
  {
    id: 'nome',
    accessorKey: 'nome',
    header: 'Título',
    cell: ({ getValue }) => <span className="font-medium text-slate-100 max-w-xs block truncate">{String(getValue())}</span>,
    size: 300,
    minSize: 90
  },
  {
    id: 'responsavel',
    accessorKey: 'responsavel',
    header: 'Responsável',
    cell: ({ row }) => {
      const resp = row.original.responsavel;
      const nomeResponsavel = resp?.email ? resp.email.split('@')[0] : "Sem dono";
      const iniciais = nomeResponsavel.substring(0, 2).toUpperCase();
      return (
        <div className="flex items-center gap-2 block truncate">
          <Avatar className="w-5 h-5 text-[9px] font-bold">
            <AvatarFallback className="bg-blue-600/20 text-blue-400 font-extrabold">{iniciais}</AvatarFallback>
          </Avatar>
          <span className="text-slate-300 capitalize">{nomeResponsavel}</span>
        </div>
      );
    },
    size: 170,
    minSize: 90
  },
  {
    id: 'prioridade',
    accessorKey: 'prioridade',
    header: 'Prioridade',
    cell: ({ getValue }) => {
      const config = getPrioridadeConfig(Number(getValue()));
      return (
        <Badge variant="outline" className={`block truncate text-[10px] px-2 py-0.5 font-semibold rounded-md ${config.classes}`}>
          {config.label}
        </Badge>
      );
    },
    size: 100,
    minSize: 90
  },
  {
    id: 'situacao',
    accessorKey: 'situacao',
    header: 'Situação',
    cell: ({ getValue }) => {
      const config = getSituacaoConfig(Number(getValue()));
      return (
        <Badge variant="outline" className={`text-[10px] px-2 py-0.5 font-medium rounded-md block truncate ${config.classes}`}>
          {config.label}
        </Badge>
      );
    },
    size: 120,
    minSize: 120,
  },
  {
    id: 'dataCriacao',
    accessorKey: 'dataCriacao',
    header: 'Data Criação',
    cell: ({ getValue }) => <span className="text-slate-400 block truncate">{new Date(String(getValue())).toLocaleDateString('pt-BR')}</span>,
    size: 120,
    minSize:100
  },
];