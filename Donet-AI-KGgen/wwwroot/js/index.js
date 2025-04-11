document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('btnGen').addEventListener('click', async () => {
        const inputData = document.getElementById('inputData').value;
        loading.start();
        const response = await fetch('/Home/Generate', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Input: inputData })
        });
        if (response.ok) {
            const data = await response.json();
            const elements = {
                nodes: data.entities.map((entity, index) => ({
                    data: {
                        id: entity.id,
                        weight: entity.weight,
                        color: lightColors[index % lightColors.length]
                    }
                })),
                edges: data.relationships.map(rel => ({
                    data: {
                        id: `${rel.source}${rel.label}${rel.target}`,
                        source: rel.source,
                        target: rel.target,
                        label: rel.label
                    }
                }))
            };
            cy.json({ elements });
            cy.layout({
                name: 'cose',
                idealEdgeLength: 100,
                nodeOverlap: 20,
                refresh: 20,
                fit: true,
                padding: 30,
                randomize: false,
                componentSpacing: 100,
                nodeRepulsion: 400000,
                edgeElasticity: 100,
                nestingFactor: 5,
                gravity: 80,
                numIter: 1000,
                initialTemp: 200,
                coolingFactor: 0.95,
                minTemp: 1.0
            }).run();
        }
        loading.end();
    });

    const lightColors = [
        'hsl(195, 80%, 85%)',
        'hsl(120, 60%, 85%)',
        'hsl(60, 80%, 85%)',
        'hsl(350, 70%, 85%)',
        'hsl(270, 50%, 85%)',
        'hsl(30, 80%, 85%)',
        'hsl(180, 60%, 85%)',
        'hsl(15, 80%, 85%)',
        'hsl(150, 60%, 85%)',
        'hsl(240, 50%, 85%)'
    ];

    const cy = cytoscape({
        container: document.getElementById('cy'),
        elements: { nodes: [], edges: [] },
        style: [
            {
                selector: 'node',
                style: {
                    'label': 'data(id)',
                    'text-valign': 'center',
                    'text-halign': 'center',
                    'font-size': '14px',
                    'font-weight': 'bold',
                    'background-color': 'data(color)',
                    'width': 'mapData(weight, 0.1, 1, 20, 120)',
                    'height': 'mapData(weight, 0.1, 1, 20, 120)'
                }
            },
            {
                selector: 'edge',
                style: {
                    'width': 2,
                    'line-color': '#909090',
                    'curve-style': 'bezier',
                    'target-arrow-shape': 'triangle',
                    'target-arrow-color': '#909090',
                    'label': 'data(label)',
                    'font-size': '12px',
                    'text-background-color': 'white',
                    'text-background-opacity': 1,
                    'text-background-padding': '3px'
                }
            }
        ]
    });

    cy.userZoomingEnabled(true);
    cy.userPanningEnabled(true);
});

const loading = (function () {
    let _loadingElement = document.createElement('div');
    _loadingElement.style.display = 'none';
    _loadingElement.classList.add('loading-element');
    _loadingElement.innerHTML = '<div class="loading-spinner"></div>';

    document.body.appendChild(_loadingElement);
    return {
        start: function () {
            _loadingElement.style.display = 'flex';
        },
        end: function () {
            _loadingElement.style.display = 'none';
        }
    };
})();