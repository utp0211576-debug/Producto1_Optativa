let usuarioActual = null;

// --- SISTEMA DE TOASTS ---
function mostrarToast(mensaje, tipo = 'success') {
    const container = document.getElementById('toastContainer');
    const toast = document.createElement('div');
    toast.className = `toast ${tipo}`;
    toast.innerText = mensaje;
    container.appendChild(toast);

    setTimeout(() => toast.classList.add('show'), 100);
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// --- MANEJO DE SESIÓN Y ROLES ---
document.getElementById('formLogin').addEventListener('submit', async (e) => {
    e.preventDefault();
    const usuario = document.getElementById('loginUsuario').value;
    const password = document.getElementById('loginPassword').value;

    try {
        const res = await fetch('/api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ usuario, password })
        });

        if (res.ok) {
            usuarioActual = await res.json();
            document.getElementById('loginOverlay').style.display = 'none';
            configurarInterfazPorRol();
            mostrarToast(`Sesión iniciada: ${usuarioActual.nombreCompleto}`);
        } else {
            mostrarToast('Credenciales incorrectas', 'error');
        }
    } catch (err) {
        mostrarToast('Error al conectar con la API', 'error');
    }
});

function configurarInterfazPorRol() {
    document.getElementById('topNavbar').style.display = 'flex';
    document.getElementById('mainLayout').style.display = 'flex';

    const title = document.getElementById('appTitle');
    const info = document.getElementById('userInfo');

    info.innerText = `${usuarioActual.nombreCompleto} • ${usuarioActual.rol}`;

    document.querySelectorAll('.rol-sec').forEach(el => el.style.display = 'none');

    if (usuarioActual.rol === 'Administrador') {
        title.innerText = 'Panel de Administración General';
        document.querySelectorAll('.admin-only').forEach(el => el.style.display = 'block');
    } else if (usuarioActual.rol === 'Almacenista') {
        title.innerText = 'Módulo de Almacén (Recepción y Productos)';
        document.querySelectorAll('.almacen-only').forEach(el => el.style.display = 'block');
    } else if (usuarioActual.rol === 'Vendedor') {
        title.innerText = 'Módulo de Ventas y Salidas';
        document.querySelectorAll('.vendedor-only').forEach(el => el.style.display = 'block');
    }

    cargarDesplegables();
    cargarProductos();
}

function cerrarSesion() {
    usuarioActual = null;
    document.getElementById('loginOverlay').style.display = 'flex';
    document.getElementById('topNavbar').style.display = 'none';
    document.getElementById('mainLayout').style.display = 'none';
    mostrarToast('Sesión finalizada', 'error');
}

// --- CARGA DE DATOS ---
async function cargarDesplegables() {
    try {
        const [resCat, resProv] = await Promise.all([
            fetch('/api/categorias'),
            fetch('/api/proveedores')
        ]);

        if (resCat.ok) {
            const cats = await resCat.json();
            const selCat = document.getElementById('prodCat');
            if (selCat) {
                selCat.innerHTML = '<option value="">Seleccionar Categoría</option>';
                cats.forEach(c => selCat.innerHTML += `<option value="${c.idCategoria}">${c.nombre}</option>`);
            }
        }

        if (resProv.ok) {
            const provs = await resProv.json();
            const selProv = document.getElementById('prodProv');
            if (selProv) {
                selProv.innerHTML = '<option value="">Seleccionar Proveedor</option>';
                provs.forEach(p => selProv.innerHTML += `<option value="${p.idProveedor}">${p.nombreEmpresa}</option>`);
            }
        }
    } catch (err) { console.error(err); }
}

async function cargarProductos() {
    try {
        const res = await fetch('/api/productos');
        if (!res.ok) return;
        const productos = await res.json();

        const tbody = document.getElementById('tablaProductos');
        const selEnt = document.getElementById('entProducto');
        const selSal = document.getElementById('salProducto');
        const listBajo = document.getElementById('listaStockBajo');

        tbody.innerHTML = '';
        if (selEnt) selEnt.innerHTML = '<option value="">Elegir producto...</option>';
        if (selSal) selSal.innerHTML = '<option value="">Elegir producto...</option>';
        listBajo.innerHTML = '';

        let contadorAlertas = 0;
        const puedeEliminar = usuarioActual && (usuarioActual.rol === 'Administrador' || usuarioActual.rol === 'Almacenista');

        productos.forEach(p => {
            const esStockBajo = p.stockActual <= p.stockMinimo;

            if (esStockBajo) {
                contadorAlertas++;
                listBajo.innerHTML += `<li>⚠️ <strong>${p.nombre}</strong> (Disponibles: ${p.stockActual} / Umbral: ${p.stockMinimo})</li>`;
            }

            const botonEliminar = puedeEliminar
                ? `<button class="btn-custom btn-danger" style="padding: 4px 10px; font-size: 0.75em; width: auto;" onclick="eliminarProducto(${p.idProducto}, '${p.nombre.replace(/'/g, "\\'")}')">🗑️ Eliminar</button>`
                : '<span style="color: var(--text-muted); font-size: 0.8em;">N/A</span>';

            tbody.innerHTML += `
                <tr>
                    <td>#${p.idProducto}</td>
                    <td><code>${p.codigoBarras || 'N/A'}</code></td>
                    <td><strong>${p.nombre}</strong></td>
                    <td>${p.categoria || 'Sin categoría'}</td>
                    <td><span class="status-tag ${esStockBajo ? 'tag-warn' : 'tag-ok'}">${p.stockActual} unidades</span></td>
                    <td>$${(p.precioVenta || 0).toFixed(2)}</td>
                    <td>${botonEliminar}</td>
                </tr>
            `;

            if (selEnt) selEnt.innerHTML += `<option value="${p.idProducto}">${p.nombre}</option>`;
            if (selSal) selSal.innerHTML += `<option value="${p.idProducto}">${p.nombre}</option>`;
        });

        if (contadorAlertas === 0) {
            listBajo.innerHTML = '<li>✨ El inventario está saludable. No hay productos críticos.</li>';
        }
    } catch (err) { mostrarToast('Error al cargar inventario', 'error'); }
}

// --- ELIMINACIÓN DE PRODUCTO ---
async function eliminarProducto(idProducto, nombreProducto) {
    if (!confirm(`¿Estás seguro de que deseas eliminar el producto "${nombreProducto}"?`)) {
        return;
    }

    try {
        const res = await fetch(`/api/productos/${idProducto}`, {
            method: 'DELETE'
        });

        if (res.ok) {
            mostrarToast(`Producto "${nombreProducto}" eliminado correctamente`);
            cargarProductos();
        } else {
            const errData = await res.json().catch(() => ({}));
            mostrarToast(errData.message || 'No se pudo eliminar el producto', 'error');
        }
    } catch (err) {
        mostrarToast('Error al procesar la solicitud de eliminación', 'error');
    }
}

// --- FORMULARIOS ---

// REGISTRAR CATEGORÍA (ADMIN Y ALMACENISTA)
document.getElementById('formCategoria')?.addEventListener('submit', async (e) => {
    e.preventDefault();
    const cat = {
        nombre: document.getElementById('catNombre').value,
        descripcion: document.getElementById('catDesc').value || null
    };

    const res = await fetch('/api/categorias', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(cat)
    });

    if (res.ok) {
        mostrarToast('Categoría creada exitosamente');
        document.getElementById('formCategoria').reset();
        cargarDesplegables();
    } else { mostrarToast('Error al registrar categoría', 'error'); }
});

// REGISTRAR PRODUCTO (ADMIN Y ALMACENISTA)
document.getElementById('formProducto')?.addEventListener('submit', async (e) => {
    e.preventDefault();
    const prod = {
        codigoBarras: document.getElementById('prodCodigo').value || null,
        nombre: document.getElementById('prodNombre').value,
        descripcion: document.getElementById('prodDesc').value || null,
        idCategoria: document.getElementById('prodCat').value ? parseInt(document.getElementById('prodCat').value) : null,
        idProveedor: document.getElementById('prodProv').value ? parseInt(document.getElementById('prodProv').value) : null,
        precioCompra: parseFloat(document.getElementById('prodCompra').value),
        precioVenta: parseFloat(document.getElementById('prodVenta').value),
        stockMinimo: parseInt(document.getElementById('prodMinimo').value)
    };

    const res = await fetch('/api/productos', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(prod)
    });

    if (res.ok) {
        mostrarToast('Producto registrado exitosamente');
        document.getElementById('formProducto').reset();
        cargarProductos();
    } else { mostrarToast('Error al registrar producto', 'error'); }
});

// PROCESAR MOVIMIENTO DE STOCK
async function procesarMovimiento(idProd, tipo, cant, motivo, formId) {
    const res = await fetch('/api/inventario/movimiento', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            idProducto: parseInt(idProd),
            tipoMovimiento: tipo,
            cantidad: parseInt(cant),
            motivo: motivo
        })
    });

    if (res.ok) {
        mostrarToast(`Movimiento ${tipo} registrado correctamente`);
        document.getElementById(formId).reset();
        cargarProductos();
    } else { mostrarToast('No se pudo procesar el movimiento', 'error'); }
}

// ENTRADAS (ALMACENISTA Y ADMIN)
document.getElementById('formEntrada')?.addEventListener('submit', (e) => {
    e.preventDefault();
    procesarMovimiento(
        document.getElementById('entProducto').value,
        'ENTRADA',
        document.getElementById('entCantidad').value,
        document.getElementById('entMotivo').value,
        'formEntrada'
    );
});

// SALIDAS (VENDEDOR Y ADMIN)
document.getElementById('formSalida')?.addEventListener('submit', (e) => {
    e.preventDefault();
    procesarMovimiento(
        document.getElementById('salProducto').value,
        'SALIDA',
        document.getElementById('salCantidad').value,
        document.getElementById('salMotivo').value,
        'formSalida'
    );
});