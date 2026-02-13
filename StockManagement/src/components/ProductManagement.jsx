import { useState, useEffect } from 'react';
import { productService } from "../api";
import { toast } from 'react-toastify';
import './ProductManagement.css';

function ProductManagement() {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [editingProduct, setEditingProduct] = useState(null);
  
  const [sortConfig, setSortConfig] = useState({ key: 'id', direction: 'asc' });

  const [formData, setFormData] = useState({
    price: '',
    loadDate: '',
    category: 'PRODUNO'
  });

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    setLoading(true);
    try {
      const data = await productService.getAll();
      setProducts(data);
    } catch (error) {
      console.error('Error al cargar productos:', error);
      toast.error('Error al cargar los productos');
    } finally {
      setLoading(false);
    }
  };

  // --- LÓGICA DE ORDENAMIENTO ---
  const handleSort = (key) => {
    let direction = 'asc';
    if (sortConfig.key === key && sortConfig.direction === 'asc') {
      direction = 'desc';
    }
    setSortConfig({ key, direction });
  };

  const getSortedProducts = () => {
    const sortableProducts = [...products];
    return sortableProducts.sort((a, b) => {
      let valA = a[sortConfig.key];
      let valB = b[sortConfig.key];

      // Manejo especial para fechas si es necesario
      if (sortConfig.key === 'loadDate') {
        valA = new Date(valA.split('/').reverse().join('-'));
        valB = new Date(valB.split('/').reverse().join('-'));
      }

      if (valA < valB) return sortConfig.direction === 'asc' ? -1 : 1;
      if (valA > valB) return sortConfig.direction === 'asc' ? 1 : -1;
      return 0;
    });
  };

  const sortedProducts = getSortedProducts();

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!formData.price || !formData.loadDate) {
      toast.error('Por favor complete todos los campos');
      return;
    }
    const productData = {
      price: parseFloat(formData.price),
      loadDate: new Date(formData.loadDate).toISOString(),
      category: formData.category
    };
    try {
      if (editingProduct) {
        await productService.update(editingProduct.id, productData);
        toast.success('Producto actualizado exitosamente');
      } else {
        await productService.create(productData);
        toast.success('Producto creado exitosamente');
      }
      closeModal();
      loadProducts();
    } catch (error) {
      toast.error('Error al guardar el producto');
    }
  };

  const handleEdit = (product) => {
    setEditingProduct(product);
    const dateStr = product.loadDate.split('/').reverse().join('-');
    setFormData({ price: product.price, loadDate: dateStr, category: product.category });
    setShowModal(true);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('¿Está seguro de eliminar este producto?')) return;
    try {
      await productService.delete(id);
      toast.success('Producto eliminado exitosamente');
      loadProducts();
    } catch (error) {
      toast.error('Error al eliminar el producto');
    }
  };

  const openModal = () => {
    setEditingProduct(null);
    setFormData({ price: '', loadDate: '', category: 'PRODUNO' });
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingProduct(null);
  };

  return (
    <div className="product-management">
      <div className="page-header">
        <h2>Gestión de Productos</h2>
        <button onClick={openModal} className="btn-primary">
          + Nuevo Producto
        </button>
      </div>

      {loading ? (
        <div className="loading">Cargando productos...</div>
      ) : (
        <div className="table-container">
          <table className="products-table">
            <thead>
              <tr>
                {/* CABECERAS CON CLIC PARA ORDENAR */}
                <th onClick={() => handleSort('id')} className="sortable-th">
                  ID {sortConfig.key === 'id' ? (sortConfig.direction === 'asc' ? '🔼' : '🔽') : '↕️'}
                </th>
                <th onClick={() => handleSort('price')} className="sortable-th">
                  Precio {sortConfig.key === 'price' ? (sortConfig.direction === 'asc' ? '🔼' : '🔽') : '↕️'}
                </th>
                <th onClick={() => handleSort('loadDate')} className="sortable-th">
                  Fecha de Carga {sortConfig.key === 'loadDate' ? (sortConfig.direction === 'asc' ? '🔼' : '🔽') : '↕️'}
                </th>
                <th onClick={() => handleSort('category')} className="sortable-th">
                  Categoría {sortConfig.key === 'category' ? (sortConfig.direction === 'asc' ? '🔼' : '🔽') : '↕️'}
                </th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {sortedProducts.length === 0 ? (
                <tr><td colSpan="5" className="no-data">No hay productos registrados</td></tr>
              ) : (
                sortedProducts.map(product => (
                  <tr key={product.id}>
                    <td>{product.id}</td>
                    <td>${product.price.toFixed(2)}</td>
                    <td>{product.loadDate}</td>
                    <td>
                      <span className={`badge ${product.category.toLowerCase()}`}>
                        {product.category}
                      </span>
                    </td>
                    <td>
                      <div className="action-buttons">
                        <button onClick={() => handleEdit(product)} className="btn-edit">✏️</button>
                        <button onClick={() => handleDelete(product.id)} className="btn-delete">🗑️</button>
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      )}

      {/* ... (Mantenemos tu bloque de modal igual) ... */}
      {showModal && (
        <div className="modal-overlay" onClick={closeModal}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>{editingProduct ? 'Editar Producto' : 'Nuevo Producto'}</h3>
              <button onClick={closeModal} className="modal-close">×</button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Precio</label>
                <input type="number" name="price" step="0.01" value={formData.price} onChange={handleInputChange} required />
              </div>
              <div className="form-group">
                <label>Fecha de Carga</label>
                <input type="date" name="loadDate" value={formData.loadDate} onChange={handleInputChange} required />
              </div>
              <div className="form-group">
                <label>Categoría</label>
                <select name="category" value={formData.category} onChange={handleInputChange} required>
                  <option value="PRODUNO">PRODUNO</option>
                  <option value="PRODDOS">PRODDOS</option>
                </select>
              </div>
              <div className="modal-actions">
                <button type="button" onClick={closeModal} className="btn-secondary">Cancelar</button>
                <button type="submit" className="btn-primary">{editingProduct ? 'Actualizar' : 'Crear'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default ProductManagement;