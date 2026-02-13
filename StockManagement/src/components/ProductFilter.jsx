import { useState } from 'react';
import { productService } from "../api";
import { toast } from 'react-toastify';
import './ProductFilter.css';

function ProductFilter() {
  const [budget, setBudget] = useState('');
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const budgetValue = parseInt(budget);

    if (!budget || isNaN(budgetValue)) {
      toast.error('Por favor ingrese un presupuesto válido');
      return;
    }

    if (budgetValue < 1 || budgetValue > 1000000) {
      toast.error('El presupuesto debe estar entre 1 y 1.000.000');
      return;
    }

    setLoading(true);

    try {
      const data = await productService.getFiltered(budgetValue);
      setResult(data);
      
      if (data.productOne && data.productTwo) {
        toast.success('Productos encontrados exitosamente');
      } else {
        toast.warning(data.message);
      }
    } catch (error) {
      console.error('Error al filtrar productos:', error);
      toast.error('Error al buscar productos');
    } finally {
      setLoading(false);
    }
  };

  const handleReset = () => {
    setBudget('');
    setResult(null);
  };

  return (
    <div className="product-filter">
      <div className="filter-header">
        <h2>Búsqueda de Productos por Presupuesto</h2>
        <p>Ingrese su presupuesto y encontraremos la mejor combinación de productos</p>
      </div>

      <div className="filter-card">
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="budget">Presupuesto (1 - 1.000.000)</label>
            <div className="input-with-prefix">
              <span className="prefix">$</span>
              <input
                type="number"
                id="budget"
                value={budget}
                onChange={(e) => setBudget(e.target.value)}
                placeholder="Ingrese el monto"
                min="1"
                max="1000000"
                disabled={loading}
              />
            </div>
          </div>

          <div className="button-group">
            <button
              type="button"
              onClick={handleReset}
              className="btn-secondary"
              disabled={loading || !result}
            >
              Limpiar
            </button>
            <button
              type="submit"
              className="btn-primary"
              disabled={loading}
            >
              {loading ? 'Buscando...' : 'Buscar Productos'}
            </button>
          </div>
        </form>
      </div>

      {result && (
        <div className="results-container">
          {result.productOne && result.productTwo ? (
            <>
              <div className="results-header">
                <h3>Productos Encontrados</h3>
                <div className="total-badge">
                  Total: ${result.total.toFixed(2)}
                </div>
              </div>

              <div className="products-grid">
                <div className="product-card">
                  <div className="product-label">
                    <span className="badge produno">PRODUNO</span>
                  </div>
                  <div className="product-details">
                    <div className="detail-row">
                      <span className="detail-label">ID:</span>
                      <span className="detail-value">{result.productOne.id}</span>
                    </div>
                    <div className="detail-row">
                      <span className="detail-label">Precio:</span>
                      <span className="detail-value price">
                        ${result.productOne.price.toFixed(2)}
                      </span>
                    </div>
                    <div className="detail-row">
                      <span className="detail-label">Fecha:</span>
                      <span className="detail-value">{result.productOne.loadDate}</span>
                    </div>
                  </div>
                </div>

                <div className="plus-sign">+</div>

                <div className="product-card">
                  <div className="product-label">
                    <span className="badge proddos">PRODDOS</span>
                  </div>
                  <div className="product-details">
                    <div className="detail-row">
                      <span className="detail-label">ID:</span>
                      <span className="detail-value">{result.productTwo.id}</span>
                    </div>
                    <div className="detail-row">
                      <span className="detail-label">Precio:</span>
                      <span className="detail-value price">
                        ${result.productTwo.price.toFixed(2)}
                      </span>
                    </div>
                    <div className="detail-row">
                      <span className="detail-label">Fecha:</span>
                      <span className="detail-value">{result.productTwo.loadDate}</span>
                    </div>
                  </div>
                </div>
              </div>

              <div className="result-summary">
                <p>
                  ✓ Esta es la mejor combinación de productos que no excede su presupuesto
                  de <strong>${parseInt(budget).toLocaleString()}</strong>
                </p>
                <p>
                  Ahorro: <strong>${(parseInt(budget) - result.total).toFixed(2)}</strong>
                </p>
              </div>
            </>
          ) : (
            <div className="no-results">
              <div className="no-results-icon">⚠️</div>
              <h3>No se encontraron productos</h3>
              <p>{result.message}</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
}

export default ProductFilter;