import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ProductManagement from './ProductManagement';
import ProductFilter from './ProductFilter';
import './Dashboard.css';

function Dashboard() {
  const [activeTab, setActiveTab] = useState('management');
  const navigate = useNavigate();
  const username = localStorage.getItem('username');

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    navigate('/');
  };

  return (
    <div className="dashboard">
      <nav className="navbar">
        <div className="navbar-brand">
          <h1>📦 Stock Management</h1>
        </div>
        <div className="navbar-user">
          <span className="user-greeting">Hola, {username}!</span>
          <button onClick={handleLogout} className="btn-logout">
            Cerrar Sesión
          </button>
        </div>
      </nav>

      <div className="dashboard-container">
        <div className="sidebar">
          <button
            className={`tab-button ${activeTab === 'management' ? 'active' : ''}`}
            onClick={() => setActiveTab('management')}
          >
            <span className="tab-icon">📋</span>
            <span className="tab-text">Gestión de Productos</span>
          </button>
          <button
            className={`tab-button ${activeTab === 'filter' ? 'active' : ''}`}
            onClick={() => setActiveTab('filter')}
          >
            <span className="tab-icon">🔍</span>
            <span className="tab-text">Búsqueda por Presupuesto</span>
          </button>
        </div>

        <div className="content">
          {activeTab === 'management' ? <ProductManagement /> : <ProductFilter />}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;