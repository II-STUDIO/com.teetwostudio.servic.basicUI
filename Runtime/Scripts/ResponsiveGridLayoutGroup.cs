using UnityEngine;
using UnityEngine.UI;

public class ResponsiveGridLayoutGroup : GridLayoutGroup
{
    [SerializeField] private bool applyWidth = true;
    [SerializeField] private bool applyHeight = true;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        CalculateCellConstraint();
    }

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputVertical();

        CalculateCellConstraint();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        CalculateCellConstraint();
    }

    protected override void OnTransformChildrenChanged()
    {
        base.OnTransformChildrenChanged();

        CalculateCellConstraint();

    }

    protected override void OnCanvasHierarchyChanged()
    {
        base.OnCanvasHierarchyChanged();

        CalculateCellConstraint();

    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        CalculateCellConstraint();

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        CalculateCellConstraint();
    }

    private void CalculateCellConstraint()
    {
        CalculateFixedColumn();
        CalculateFixedRow();
    }

    private void CalculateFixedColumn()
    {
        if (constraint != Constraint.FixedColumnCount)
            return;

        int columCount = constraintCount;

        if (columCount < 1)
            columCount = 1;

        float width = rectTransform.rect.width;
        float spacingValue = spacing.x * columCount;

        ApplySize(constraintCount, width, spacingValue);
    }

    private void CalculateFixedRow()
    {
        if (constraint != Constraint.FixedRowCount)
            return;

        int rawCount = constraintCount;

        if (rawCount < 1)
            rawCount = 1;

        float height = rectTransform.rect.height;
        float spacingValue = spacing.y * rawCount;

        ApplySize(rawCount, height, spacingValue);
    }

    private void ApplySize(int constraintCount, float constraintSize, float spacingValue)
    {
        float resultArea = constraintSize - spacingValue;

        float resultSize = resultArea / constraintCount;

        Vector2 size = cellSize;

        if (applyWidth)
            size.x = resultSize;

        if (applyHeight)
            size.y = resultSize;

        cellSize = size;
    }
}
