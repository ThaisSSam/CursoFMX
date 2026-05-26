import React, { Suspense, lazy } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import AppLoadingFallback from './components/layout/AppLoadingFallback';
import App from './App';
import EsqueciSenha from './screens/login/esqueciSenha';

const DashboardScreen = lazy(() => import('./screens/dashboard'));

interface PrivateRouteProps { children: React.ReactNode; }

const PrivateRoute = ({ children }: PrivateRouteProps) => {
  const token = localStorage.getItem("auth_token");
  return token ? <>{children}</> : <Navigate to="/login" replace />;
};

export const AppRoutes = () => {
  return (
    <Suspense fallback={<AppLoadingFallback />}>
      <Routes>
        <Route path="/login" element={<App />} />
        {/* Nova Rota Cadastrada */}
        <Route path="/forgot-password" element={<EsqueciSenha />} />

        <Route path="/home" element={
          <PrivateRoute>
            <DashboardScreen />
          </PrivateRoute>
        } />
        <Route path="/" element={<Navigate to="/home" replace />} />
        <Route path="*" element={<Navigate to="/home" replace />} />
      </Routes>
    </Suspense>
  );
};